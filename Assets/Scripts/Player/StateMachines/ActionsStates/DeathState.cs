using UnityEngine;

public class DeathState : TemplateState
{
    
    protected override void OnStateInit()
    {
    }
    protected override void OnStateEnter(TemplateState previousState)
    {
        SoundManager.instance.PlayClip("death");
        foreach(SpriteRenderer sprite in StateMachine.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = false;
        }
        StateMachine._iMouvementLockedWriter.isMouvementLocked = true;

        StateMachine.bouleMouvement.gameObject.SetActive(false);
        StateMachine.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }
    protected override void OnStateUpdate()
    {
        if (!StateMachine._iMouvementLockedReader.isMouvementLocked)
        {
            StateMachine.ChangeState(StateMachine.stateIdle);
        }
    }
}
