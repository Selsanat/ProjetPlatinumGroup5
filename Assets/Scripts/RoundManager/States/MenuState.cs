using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class MenuState : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        StateMachine.HideAllMenusExceptThis(ui);
    }

    protected override void OnStateUpdate()
    {

    }
}
