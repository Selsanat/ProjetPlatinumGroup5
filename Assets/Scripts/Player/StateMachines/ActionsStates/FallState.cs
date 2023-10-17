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
        if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.down, StateMachine.transform, StateMachine.velocity*Time.deltaTime))
        {
            StateMachine.velocity.y = 0;
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }
        else
        {

            float h = _movementParams.jumpMaxHeight;
            float th = _movementParams.jumpDuration / 2;
            float g = -(2 * h) / Mathf.Pow(th, 2);

            StateMachine.velocity.y += g * Time.deltaTime;
        }

        _characterController.Move(StateMachine.velocity);
    }
}
