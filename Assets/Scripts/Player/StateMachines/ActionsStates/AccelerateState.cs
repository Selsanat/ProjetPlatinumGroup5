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

        #region JumpBuffer 
        if (_iWantsJumpWriter.jumpBuffer > 0) _iWantsJumpWriter.jumpBuffer -= Time.deltaTime;
        if (_iWantsJumpWriter.wantsJump) _iWantsJumpWriter.jumpBuffer = _movementParams.jumpBuffer;
        #endregion

        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, Vector3.zero))
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }
        if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x)*Vector2.right, StateMachine.transform, Vector2.zero)||StateMachine.velocity.x ==0 && _IOrientWriter.orient.x ==0)
        {
            StateMachine.velocity.x = 0;
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }
        if (_iWantsJumpWriter.wantsJump || _iWantsJumpWriter.jumpBuffer > 0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        Debug.Log(Mathf.Abs(StateMachine.velocity.x) + " Velocity and Max speed is : " + _movementParams.maxSpeed/10);
        if (Mathf.Abs(StateMachine.velocity.x) >= _movementParams.maxSpeed/10)
        {
            StateMachine.ChangeState(StateMachine.stateWalk);
            return;
        }

        _timer += Time.deltaTime;

        StateMachine.velocity.x = (_timer / _movementParams.accelerationTime)* (_movementParams.maxSpeed/10 * _IOrientWriter.orient.x);

        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
