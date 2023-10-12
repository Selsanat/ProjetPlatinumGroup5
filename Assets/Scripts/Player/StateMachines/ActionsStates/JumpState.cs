using System.Collections;
using System.Collections.Generic;
using DetectCollisionExtension;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
public class JumpState : TemplateState
{
    private float _timer;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = 0;
    }

    protected override void OnStateUpdate()
    {

        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, StateMachine.velocity)&& StateMachine.velocity.y <= 0)
        {
                ChangeState(StateMachine.fallState);
                return;
        }

        _timer += Time.deltaTime;


        float percent = _timer / movementParams.timeToReachJumpHeight;
        StateMachine.velocity.y = movementParams.jumpHeight - movementParams.timeToReachJumpHeight * percent;
        StateMachine.velocity.x += Time.deltaTime / _movementParams.accelerationTime * _IOrientWriter.orient.x * (_movementParams.airControl);
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -_movementParams.maxSpeed , _movementParams.maxSpeed );
        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
