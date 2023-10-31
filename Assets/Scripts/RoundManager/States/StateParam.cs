using UnityEngine;

public class StateParam : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        Debug.Log(ui);
        StateMachine.HideAllMenusExceptThis(ui);
    }

    protected override void OnStateUpdate()
    {

    }
}
