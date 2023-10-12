using System.Collections;
using System.Collections.Generic;
using DetectCollisionExtension;
using UnityEngine;

public class DecelerateState : TemplateState
{
    private float sign;
    private float _timer;

    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = (1-(Mathf.Abs(StateMachine.velocity.x / _movementParams.maxSpeed)))* _movementParams.decelerationTime;
        sign = Mathf.Sign(StateMachine.velocity.x);

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

            #region ToIdle

            if (_timer >= _movementParams.decelerationTime)
            {
                StateMachine.ChangeState(StateMachine.stateIdle);
                return;
            }
            #endregion
        }
        else
        {
            StateMachine.ChangeState(StateMachine.stateAccelerate);
        }
        #endregion

        _timer += Time.deltaTime;
        Debug.Log(_timer + " Temps atm, et temps de deceleration : " + _movementParams.decelerationTime);
        StateMachine.velocity.x = (1- _timer) * (_movementParams.maxSpeed * Vector2.right.x* sign);
        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
