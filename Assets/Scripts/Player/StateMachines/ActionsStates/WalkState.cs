using DetectCollisionExtension;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkState : TemplateState
{

    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        StateMachine.velocity.x = _movementParams.maxSpeed* Mathf.Sign(StateMachine.velocity.x);
    }

    protected override void OnStateUpdate()
    {
        #region Jump

        if (_iWantsJumpWriter.wantsJump || _iWantsJumpWriter.jumpBuffer > 0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        #endregion

        #region Fall
        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, Vector3.zero))
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }
        #endregion

        #region StopInput
        if (_IOrientWriter.orient.x == 0)
        {
            #region HasHitWall
            if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.right, StateMachine.transform, Vector2.zero))
            {
                StateMachine.velocity.x = 0;
                StateMachine.ChangeState(StateMachine.stateIdle);
                return;
            }
            #endregion

            #region Decelerate
            StateMachine.ChangeState(StateMachine.stateDecelerate);
            return; 
            #endregion
        }
        #endregion


        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
