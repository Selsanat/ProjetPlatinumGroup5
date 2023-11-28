using UnityEngine;

public abstract class TemplateState
{
    public PlayerStateMachine StateMachine { get; private set; }

    protected MovementParams movementParams => StateMachine.movementsParam;
    protected IOrientWriter _IOrientWriter => StateMachine.GetComponent<IOrientWriter>();
    protected IOrientReader _IOrientReader => StateMachine.GetComponent<IOrientReader>();
    protected IWantsJumpWriter _iWantsJumpWriter => StateMachine.GetComponent<IWantsJumpWriter>();
    protected Animator charAnimator => StateMachine.AnimatorPerso;

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

        charAnimator.SetBool("Run", StateMachine.velocity.x != 0);
        if (_IOrientReader.orient.x != 0)
        {
            Vector3 myScale = charAnimator.transform.localScale;
            myScale.x = Mathf.Abs(myScale.x) * Mathf.Sign(StateMachine.velocity.x);
            charAnimator.transform.localScale = myScale;
        }
        if (StateMachine.CoyoteWindow>0) StateMachine.CoyoteWindow -= Time.deltaTime;

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
