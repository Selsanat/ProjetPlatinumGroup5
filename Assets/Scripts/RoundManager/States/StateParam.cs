using UnityEngine;

public class StateParam : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        StateMachine.HideAllMenusExceptThis(ui, true);
    }

    protected override void OnStateUpdate()
    {

    }
}
