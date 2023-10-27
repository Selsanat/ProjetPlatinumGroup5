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
        List<InputDevice> devices = new List<InputDevice>();
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad || device is Keyboard)
            {
                devices.Add(device);
            }
        }
        StateMachine.HideAllMenusExceptThis(ui);
        for(int i =0; i< managerManager.gameParams.NombreJoueurs;i++)
        {
            var player = inputsManager.playerInputManager.JoinPlayer(-1,-1,null, devices[i]);
            player.transform.position = GameObject.FindGameObjectsWithTag("SpawnPoints")[i].transform.position;
            PlayerStateMachine playerStateMachine = player.GetComponent<PlayerStateMachine>();
            //playerStateMachine.CurrentState.LockMouvement();
            inputsManager.PlayersStateMachines.Add(playerStateMachine);
           
        }
        
        cam = Camera.main;
        Debug.Log(cam);
        DOTween.Init();
        Vector3 StartPos = cam.transform.position;
        Sequence mySequence = DOTween.Sequence();
        foreach (var player in inputsManager.PlayersStateMachines)
        {
            Vector3 pos = player.transform.position;
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
        foreach (var player in inputsManager.PlayersStateMachines)
        {
           // player.CurrentState.UnlockMouvement();
        }
    }
    protected override void OnStateUpdate()
    {
    }
}