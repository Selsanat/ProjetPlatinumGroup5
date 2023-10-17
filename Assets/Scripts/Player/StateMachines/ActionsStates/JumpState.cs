using System.Collections;
using System.Collections.Generic;
using DetectCollisionExtension;
using Unity.VisualScripting;
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

        float h = _movementParams.jumpMaxHeight;
        float th = _movementParams.jumpDuration / 2;

        StateMachine.velocity.y = 2 * h / th;
    }

    protected override void OnStateUpdate()
    {
        if (_timer > _movementParams.jumpDuration / 2)
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }

        _timer += Time.deltaTime;

        float h = _movementParams.jumpMaxHeight;
        float th = _movementParams.jumpDuration / 2;
        float g = -(2 * h) / Mathf.Pow(th,2);


        StateMachine.velocity.y += g * Time.deltaTime;

        _characterController.Move(StateMachine.velocity);
    }
}
