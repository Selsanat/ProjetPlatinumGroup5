using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DetectCollisionExtension;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Vector2 = UnityEngine.Vector2;

public class FallState : TemplateState
{
    private float _timer;  
    private float _coyote;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        StateMachine.velocity.y = 0;
    }

    protected override void OnStateUpdate()
    {
        if (DetectCollision.isColliding(Mathf.Abs(StateMachine.velocity.y) * Vector2.down, StateMachine.transform,Vector2.zero))
        {
            StateMachine.velocity.y = 0;

            if (_IOrientWriter.orient.x != 0)
            {
                StateMachine.ChangeState(StateMachine.stateAccelerate);
                return;
            }
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }


        float h = _movementParams.jumpMaxHeight;
        float th = _movementParams.fallDuration / 2;
        float g = -(2 * h) / Mathf.Pow(th, 2);

        StateMachine.velocity.y += g * Time.deltaTime;
        //StateMachine.velocity.y =- _movementParams.gravityScale;
        _characterController.Move(StateMachine.velocity);
    }
}
