using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class StateRound : GameStateTemplate
{
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
        Debug.Log("Entré");
        StateMachine.HideAllMenusExceptThis(ui);
        for(int i =0; i<ManagerManager.Instance.gameParams.NombreJoueurs;i++)
        {
           Debug.Log( InputSystem.devices[i]);
            var player = InputsManager.Instance.inputmanager.JoinPlayer(-1,-1,null, devices[i]);
            player.transform.position = GameObject.FindGameObjectsWithTag("SpawnPoints")[i].transform.position;
        }
    }

    protected override void OnStateUpdate()
    {
    }
}