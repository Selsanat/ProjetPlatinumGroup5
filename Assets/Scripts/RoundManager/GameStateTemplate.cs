using UnityEngine;

public abstract class GameStateTemplate
{
    protected GameStateMachine StateMachine { get; private set; }
    protected MenuManager manager => MenuManager.Instance;
    protected ManagerManager managerManager => ManagerManager.Instance;
    protected InputsManager inputsManager => InputsManager.Instance;
    protected void ChangeState(GameStateTemplate state) => StateMachine.ChangeState(state);
    [HideInInspector]
    public GameObject ui;

    public void Init(GameStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        OnStateInit();
    }

    public void StateEnter(GameStateTemplate previousState) => OnStateEnter(previousState);
    public void StateExit(GameStateTemplate nextState) => OnStateExit(nextState);
    public void StateUpdate() => OnStateUpdate();
    protected virtual void OnStateInit() { }

    protected virtual void OnStateEnter(GameStateTemplate previousState)
    {
        StateMachine.HideAllMenusExceptThis(ui);
    }
    protected virtual void OnStateExit(GameStateTemplate nextState) { }
    protected virtual void OnStateUpdate() { }


}
