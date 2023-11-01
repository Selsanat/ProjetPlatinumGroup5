public class DeathState : TemplateState
{
    
    protected override void OnStateInit()
    {
    }
    protected override void OnStateEnter(TemplateState previousState)
    {
        animator.Play("Death");
        StateMachine._iMouvementLockedWriter.isMouvementLocked = true;

    }
    protected override void OnStateUpdate()
    {
        if (!StateMachine._iMouvementLockedReader.isMouvementLocked)
        {
            StateMachine.ChangeState(StateMachine.stateIdle);
        }
    }

}
