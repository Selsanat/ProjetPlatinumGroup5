using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuManager;

public abstract class TemplateState
{
    public PlayerStateMachine StateMachine { get; private set; }

    protected MovementParams movementParams => StateMachine.movementsParam;
    protected IOrientWriter _IOrientWriter => StateMachine.GetComponent<IOrientWriter>();
    protected IWantsJumpWriter _iWantsJumpWriter => StateMachine.GetComponent<IWantsJumpWriter>();

    protected CharacterController _characterController => StateMachine.GetComponent<CharacterController>();

    protected IMouvementLockedReader _iMouvementLockedReader => StateMachine.GetComponent<IMouvementLockedReader>();
    protected IMouvementLockedWriter _iMouvementLockedWriter => StateMachine.GetComponent<IMouvementLockedWriter>();
    protected void ChangeState(TemplateState state) => StateMachine.ChangeState(state);

    protected MovementParams _movementParams => StateMachine.movementsParam;
    public void Init(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        OnStateInit();
        //_IOrientWriter = StateMachine.GetComponent<IOrientWriter>();
    }

    public void StateEnter(TemplateState previousState) => OnStateEnter(previousState);
    public void StateExit(TemplateState nextState) => OnStateExit(nextState);
    public void StateUpdate() => OnStateUpdate();
    protected virtual void OnStateInit() { }
    protected virtual void OnStateEnter(TemplateState previousState) { }
    protected virtual void OnStateExit(TemplateState nextState) { }
    protected virtual void OnStateUpdate() { }
    public void LockMouvement() => _iMouvementLockedWriter.isMouvementLocked = true;
    public void UnlockMouvement() => _iMouvementLockedWriter.isMouvementLocked = false;



}
