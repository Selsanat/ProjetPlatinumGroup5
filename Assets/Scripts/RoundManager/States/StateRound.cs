using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StateRound : GameStateTemplate
{
    private Camera cam;
    private CameraParams cameraParams;

    public bool _isPaused = false;
    DepthOfField dof;
    HorizontalLayoutGroup horizontalLayoutGroup => ManagerManager.Instance.horizontalLayoutGroup;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        SoundManager.instance.Pauseclip("Drill");
        SoundManager.instance.PlayARandomMusic();
        SoundManager.instance.PlayRandomClip("Narrator pre");
        cameraParams = CameraTransition.Instance.cameraParams;
        StateMachine.HideAllMenusExceptThis();
        RoundManager.Instance.StartRound();
        RoundManager.Instance.ShowCadrants();
        AnimationDebutDeRound();
    }


    protected override void OnStateUpdate()
    {
        
    }

    #region Animation Debut De round
    void AnimationDebutDeRound()
    {
        Volume vol = ManagerManager.Instance.Volume;
        vol.profile.TryGet<DepthOfField>(out dof);
        lockMouvements();
        CameraTransition camTrans = CameraTransition.Instance;
        cam =  camTrans.MainCam;
        float initOrthoSize = camTrans.initOrtho;

        #region SequenceDef
        Vector3 StartPos = camTrans.initPos;
        Sequence mySequence = CameraTransition.Instance.UnfreezeIt();
        mySequence.Append(DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 50, 0.5f));
        mySequence.Join(DOTween.To(() => horizontalLayoutGroup.padding.top, x => horizontalLayoutGroup.padding.top = x, 0, 0.5f));
        mySequence.Join(DOTween.To(() => horizontalLayoutGroup.spacing, x => horizontalLayoutGroup.spacing = x, -360, 0.5f));
        foreach (Transform child in horizontalLayoutGroup.transform)
        {
            mySequence.Join(child.DOScale(Vector3.one, 0.5f));
        }
        mySequence.AppendInterval(0.5f);
        mySequence.AppendCallback(() =>
        {
            RoundManager.Instance.UpdateScores();
            SoundManager.instance.PlayClip("Round Win");
        });
        mySequence.AppendInterval(0.5f);
        mySequence.Append(DOTween.To(() => horizontalLayoutGroup.padding.top, x => horizontalLayoutGroup.padding.top = x, -400, 0.5f));
        mySequence.Join(DOTween.To(() => horizontalLayoutGroup.spacing, x => horizontalLayoutGroup.spacing = x, 0, 1));
        mySequence.Join(DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 0, 0.5f));
        foreach (Transform child in horizontalLayoutGroup.transform)
        {
            mySequence.Join(child.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f));
        }
        foreach (var player in inputsManager.playerInputs)
        {
            Vector3 pos = player._playerStateMachine.transform.position;
            //A cause de la rotation de la camera
            pos.y -= (Camera.main.transform.rotation * Camera.main.transform.forward).y * Mathf.Abs(pos.x - Camera.main.transform.position.x);
            pos.z = StartPos.z;
            mySequence.Append(cam.transform.DOMove(pos, cameraParams.timeToMoveFromPlayerToPlayer, false)).SetEase(Ease.InQuad);
            mySequence.Join(cam.DOOrthoSize(cameraParams.Zoom, cameraParams.TimeToZoom).SetEase(Ease.OutSine).OnStepComplete(() =>
            {
                if (player._playerStateMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0] is Gamepad)
                    HapticsManager.Instance.Vibrate("PreRoundZoom", (Gamepad)player._playerStateMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0]);
                SoundManager.instance.PlayRandomClip("Spawn");
            }));
            mySequence.AppendInterval(cameraParams.intervalBetweenPlayers);
        }
        mySequence.Append(cam.transform.DOMove(StartPos, cameraParams.timeToMoveFromPlayerToPlayer, false));
        mySequence.Join(cam.DOOrthoSize(initOrthoSize, cameraParams.TimeToZoom));
        mySequence.AppendInterval(cameraParams.timeAfterZoomsBeforeRoundStart);
        mySequence.OnComplete(() =>
        {
            makeMainCameraSameAsTransi();
            unlockMovements();
            CameraTransition.Instance.ResetCams();
            CameraTransition.Instance.cameraFollow.FollowPlayers = true;
        }); 
        #endregion
        CameraTransition.Instance.mySequence.Play();
       
    }
    void makeMainCameraSameAsTransi()
    {
        CameraTransition camTrans = CameraTransition.Instance;
        cam = camTrans.MainCam;
        Camera.main.transform.position = cam.transform.position;
        Camera.main.orthographicSize = cam.orthographicSize;

    }
    void unlockMovements()
    {
        foreach (var player in inputsManager.playerInputs)
        {
            player._playerStateMachine._iMouvementLockedWriter.isMouvementLocked = false;
        }
    }

    void lockMouvements()
    {
        foreach (var player in inputsManager.playerInputs)
        {
            if(player._playerStateMachine != null)
            player._playerStateMachine._iMouvementLockedWriter.isMouvementLocked = true;
        }
    }
    #endregion
}