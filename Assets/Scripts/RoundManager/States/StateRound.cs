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
        cam = Camera.main;
        DOTween.Init();
        Vector3 StartPos = cam.transform.position;
        Sequence mySequence = DOTween.Sequence();
        foreach (var player in inputsManager.playerInputs)
        {
            Vector3 pos = player._playerStateMachine.transform.position;
            pos.z = cam.transform.position.z;
            mySequence.Append(cam.transform.DOMove(pos, 1, false)).SetEase(Ease.InQuad);
            mySequence.Join(cam.DOOrthoSize(5, 1)).SetEase(Ease.OutSine);
            mySequence.AppendInterval(1);
        }
        mySequence.Append(cam.transform.DOMove(StartPos, 1, false));
        mySequence.Join(cam.DOOrthoSize(15, 1));
        mySequence.OnComplete(unlockMovements);
        mySequence.Play();
       
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