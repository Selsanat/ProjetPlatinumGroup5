using UnityEngine;

public abstract class TemplateState
{
    public PlayerStateMachine StateMachine { get; private set; }

    protected MovementParams movementParams => StateMachine.movementsParam;
    protected IOrientWriter _IOrientWriter => StateMachine.GetComponent<IOrientWriter>();
    protected IWantsJumpWriter _iWantsJumpWriter => StateMachine.GetComponent<IWantsJumpWriter>();
    protected Animator animator => StateMachine.GetComponentInChildren<Animator>();

    protected CharacterController _characterController => StateMachine.GetComponent<CharacterController>();

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
    public void StateUpdate()
    {
        OnStateUpdate();

        if(StateMachine.CoyoteWindow>0) StateMachine.CoyoteWindow -= Time.deltaTime;
        if (StateMachine._iMouvementLockedReader.isMouvementLocked)
        {
            StateMachine.velocity.x = 0;
            StateMachine.velocity.y = 0;
        }

        if (_iWantsJumpWriter.wantsJump && StateMachine.CurrentState != StateMachine.jumpState)
        {
            StateMachine.JumpBuffer = movementParams.JumpBuffer;
        }
        else
        {
            if (StateMachine.JumpBuffer > 0)
            {
                StateMachine.JumpBuffer -= Time.deltaTime;
                StateMachine.JumpBuffer = Mathf.Clamp(StateMachine.JumpBuffer, 0, movementParams.JumpBuffer);
            }
        }
    }

    protected virtual void OnStateInit() { }
    protected virtual void OnStateEnter(TemplateState previousState) { }
    protected virtual void OnStateExit(TemplateState nextState) { }
    protected virtual void OnStateUpdate() { }
}
