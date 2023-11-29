using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuState : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        if(ui == null) ui = StateMachine.Menus[StateMachine.AllStates.ToList().IndexOf(this)].menuObject;
        StateMachine.HideAllMenusExceptThis(ui);
        if(StateMachine.PreviousState == StateMachine.endRound)
        {
            ManagerManager.Instance.Players.Clear();
            RoundManager.Instance.players.Clear();
            RoundManager.Instance.alivePlayers.Clear();
            CameraTransition.Instance.UnfreezeIt();
            PlayerInputManager.instance.GetComponentInChildren<PlayerInput>();

        }
    }

    protected override void OnStateUpdate()
    {

    }
}
