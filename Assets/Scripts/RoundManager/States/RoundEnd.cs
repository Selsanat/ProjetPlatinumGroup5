using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using TMPro;

public class RoundEnd : GameStateTemplate
{
    private Camera cam;
    CameraParams camparam;
    Sequence mySequence;
    DepthOfField dof;
    HorizontalLayoutGroup horizontalLayoutGroup;

    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        camparam = CameraTransition.Instance.cameraParams;
        CameraTransition.Instance.cameraFollow.FollowPlayers = false;
        //StateMachine.HideAllMenusExceptThis(ui);
        cam = CameraTransition.Instance.TransitionCam;
        SoundManager.instance.PlayRandomClip("Narrator post");
        cam.DOOrthoSize(camparam.OrthoSizeRoundEnd,camparam.TimeToZoomEndRound).OnComplete(() =>
        {
            CameraTransition.Instance.CameraRotation();
        });
        StateMachine.StartCoroutine(NextRound());
    }

    protected override void OnStateUpdate()
    {
        if (cam != null && RoundManager.Instance.alivePlayers.Count>0)
        {
            if (RoundManager.Instance.alivePlayers[0]._playerStateMachine != null)
            {
                Vector3 playerPos = RoundManager.Instance.alivePlayers[0]._playerStateMachine.gameObject.transform.position;
                playerPos.z = -camparam.distanceZAlaCam;
                CameraTransition.Instance.TransitionCam.transform.DOMove(playerPos, camparam.SmoothnessZoomRoundEnd);
            }
        }
    }

    IEnumerator NextRound()
    {
        int[] points = new int[4];
        foreach (RoundManager.Player player in RoundManager.Instance.players)
        {
            points[player._playerStateMachine.team] += player._points;
            if (points[player._playerStateMachine.team] >= ManagerManager.Instance.gameParams.PointsToWin)
            {
                #region SequenceSetter
                mySequence = DOTween.Sequence();
                horizontalLayoutGroup = ManagerManager.Instance.horizontalLayoutGroup;
                Volume vol = ManagerManager.Instance.Volume;
                vol.profile.TryGet<DepthOfField>(out dof);
                mySequence.Append(DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 50, 1.5f));
                mySequence.Join(DOTween.To(() => horizontalLayoutGroup.padding.top, x => horizontalLayoutGroup.padding.top = x, -100, 1.5f));
                mySequence.Join(DOTween.To(() => horizontalLayoutGroup.spacing, x => horizontalLayoutGroup.spacing = x, -360, 1.5f));
                foreach (Transform child in horizontalLayoutGroup.transform)
                {
                    if (horizontalLayoutGroup.transform.GetChild(player._playerStateMachine.team) == child) mySequence.Join(child.DOScale(new Vector3(1.5f,1.5f,1.5f), 1.5f)) ;
                    mySequence.Join(child.DOScale(Vector3.one, 1.5f));
                }
                mySequence.AppendInterval(1f);
                mySequence.AppendInterval(1f);
                foreach (Transform child in horizontalLayoutGroup.transform)
                {
                    if (horizontalLayoutGroup.transform.GetChild(player._playerStateMachine.team) == child) mySequence.Join(child.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f));
                    else mySequence.Join(child.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f));
                }
                mySequence.AppendInterval(0.5f);
                mySequence.AppendCallback(() => {
                    RoundManager.Instance.UpdateScores();
                    SoundManager.instance.PlayClip("Win");
                });
                mySequence.AppendInterval(4f);
                #endregion

                mySequence.Play().OnComplete(() => {
                    Volume vol = RoundManager.Instance.Volume;
                    vol.profile.TryGet<ChromaticAberration>(out ChromaticAberration CA);
                    CA.intensity.value = 0;
                    
                    StateMachine.HideAllMenusExceptThis(ui, false);
                    foreach(BouleMouvement boule in GameObject.FindObjectsOfType<BouleMouvement>())
                    {
                        GameObject.Destroy(boule.gameObject);
                    }
                    foreach (GameObject gm in RoundManager.Instance.cadrants)
                    {
                        gm.SetActive(false);
                    }
                    foreach(TMP_Text gm in RoundManager.Instance.scores)
                    {
                        gm.text = "";
                    }
                    foreach(CharacterSelector selec in ManagerManager.Instance.characterSelector)
                    {
                        GameObject.Destroy(selec.gameObject);
                    }

                    RoundManager.Instance.DestroyAllPlayers();
                    RoundManager.Instance.alivePlayers.Clear();
                    ManagerManager.Instance.characterSelector.Clear();
                    SoundManager.instance.StopbackgroundMusic();
                    SoundManager.instance.ResetValues();
                    InputsManager.Instance.resetPlayers();
                });
                yield break;
            }

        }
        yield return new WaitForSeconds(ManagerManager.Instance.gameParams.TransiTimeAfterRound);
        StateMachine.StartCoroutine(RoundManager.Instance.NewRound());
    }
}