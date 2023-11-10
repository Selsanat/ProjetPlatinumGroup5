using System.Linq;
using UnityEngine;

public class MenuState : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        if(ui == null) ui = StateMachine.Menus[StateMachine.AllStates.ToList().IndexOf(this)].menuObject;
        StateMachine.HideAllMenusExceptThis(ui);
    }

    protected override void OnStateUpdate()
    {

    }
}
