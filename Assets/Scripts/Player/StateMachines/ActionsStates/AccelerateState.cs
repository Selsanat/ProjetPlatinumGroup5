using System.Collections;
using System.Collections.Generic;
using DetectCollisionExtension;
using UnityEngine;

public class AccelerateState : TemplateState
{

    private float _timer;

    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = StateMachine.velocity.x / _movementParams.maxSpeed* _movementParams.accelerationTime;
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

        #region Walk
        //Debug.Log(Mathf.Abs(StateMachine.velocity.x) + " Velocity and Max speed is : " + _movementParams.maxSpeed);
        if (Mathf.Abs(StateMachine.velocity.x) >= _movementParams.maxSpeed)
        {
            StateMachine.transform.Translate(StateMachine.velocity);
            StateMachine.ChangeState(StateMachine.stateWalk);
            return;
        } 
        #endregion

        _timer += Time.deltaTime;

        StateMachine.velocity.x = (_timer / _movementParams.accelerationTime)* (_movementParams.maxSpeed * _IOrientWriter.orient.x);
        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
