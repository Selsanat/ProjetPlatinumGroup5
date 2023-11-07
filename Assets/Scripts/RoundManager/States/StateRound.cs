using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class StateRound : GameStateTemplate
{
    private Camera cam;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        StateMachine.HideAllMenusExceptThis();
        RoundManager.Instance.StartRound();
        AnimationDebutDeRound();
    }


    protected override void OnStateUpdate()
    {
    }

    #region Animation Debut De round
    void AnimationDebutDeRound()
    {
        CameraTransition camTrans = CameraTransition.Instance;
        cam =  camTrans.MainCam;
        float initOrthoSize = camTrans.initOrtho;

        Vector3 StartPos = camTrans.initPos;
        Sequence mySequence = CameraTransition.Instance.UnfreezeIt();
        foreach (var player in inputsManager.playerInputs)
        {
            Vector3 pos = player._playerStateMachine.transform.position;
            pos.z = StartPos.z;
            mySequence.Append(cam.transform.DOMove(pos, 1, false)).SetEase(Ease.InQuad);
            mySequence.Join(cam.DOOrthoSize(5, 1).SetEase(Ease.OutSine));
            mySequence.AppendInterval(1);
        }
        mySequence.Append(cam.transform.DOMove(StartPos, 1, false));
        mySequence.Join(cam.DOOrthoSize(initOrthoSize, 1));
        mySequence.OnComplete(() =>
        {
            makeMainCameraSameAsTransi();
            unlockMovements();
            CameraTransition.Instance.ResetCams();

        });
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
    #endregion
}