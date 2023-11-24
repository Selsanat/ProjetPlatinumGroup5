using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

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
        StateMachine.HideAllMenusExceptThis(ui);
        cam = CameraTransition.Instance.TransitionCam;
        cam.DOOrthoSize(camparam.OrthoSizeRoundEnd,camparam.TimeToZoomEndRound);
        StateMachine.StartCoroutine(NextRound());
    }

    protected override void OnStateUpdate()
    {
        if (cam != null)
        {
            Vector3 playerPos = RoundManager.Instance.alivePlayers[0]._playerStateMachine.gameObject.transform.position;
            playerPos.z = -camparam.distanceZAlaCam;
            CameraTransition.Instance.TransitionCam.transform.DOMove(playerPos, camparam.SmoothnessZoomRoundEnd);
        }
    }

    IEnumerator NextRound()
    {
        foreach (RoundManager.Player player in RoundManager.Instance.players)
        {
            if (player._points >= ManagerManager.Instance.gameParams.PointsToWin)
            {
                #region SequenceSetter
                mySequence = DOTween.Sequence();
                horizontalLayoutGroup = ManagerManager.Instance.horizontalLayoutGroup;
                Volume vol = ManagerManager.Instance.Volume;
                vol.profile.TryGet<DepthOfField>(out dof);
                mySequence.Append(DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 50, 0.5f));
                mySequence.Join(DOTween.To(() => horizontalLayoutGroup.padding.top, x => horizontalLayoutGroup.padding.top = x, 0, 0.5f));
                mySequence.Join(DOTween.To(() => horizontalLayoutGroup.spacing, x => horizontalLayoutGroup.spacing = x, -360, 0.5f));
                foreach (Transform child in horizontalLayoutGroup.transform)
                {
                    mySequence.Join(child.DOScale(Vector3.one, 0.5f));
                }
                mySequence.AppendInterval(0.5f);
                mySequence.AppendCallback(() => RoundManager.Instance.UpdateScores());
                mySequence.AppendInterval(4f);
                #endregion

                mySequence.Play().OnComplete(() => {
                    RoundManager.Instance.DestroyAllPlayers();
                });
                yield break;
            }

        }
        yield return new WaitForSeconds(ManagerManager.Instance.gameParams.TransiTimeAfterRound);
        StateMachine.StartCoroutine(RoundManager.Instance.NewRound());
    }
}