using UnityEngine;
using DetectCollisionExtension;

public class IdleState : TemplateState
{
    
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        foreach (SpriteRenderer sprite in StateMachine.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = true;
        }

        if (StateMachine.bouleMouvement!= null)
        if (_IOrientWriter.orient.x==0)
        StateMachine.velocity = Vector2.zero;
        StateMachine.GetComponentInChildren<CapsuleCollider>().enabled = true;
    }

    protected override void OnStateUpdate()
    {
        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform,Vector3.zero))
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }
        else
        {
            if (StateMachine._iMouvementLockedReader.isMouvementLocked) return;
            if (_iWantsJumpWriter.wantsJump || StateMachine.JumpBuffer > 0)
            {
                StateMachine.ChangeState(StateMachine.jumpState);
                return;
            }

            if (_IOrientWriter.orient.x != 0)
            {
                StateMachine.ChangeState(StateMachine.stateAccelerate);
                return;
            }
        }


        StateMachine.velocity = Vector2.zero;
        StateMachine.transform.Translate(StateMachine.velocity*Time.time);
    }
}
