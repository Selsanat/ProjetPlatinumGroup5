using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        manager.HideAllMenusExceptThis(manager.mainMenu);
    }

    protected override void OnStateUpdate()
    {

    }
}
