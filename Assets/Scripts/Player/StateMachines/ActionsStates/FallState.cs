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
    private float _timerX;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = (StateMachine.velocity.y/_movementParams.maxFallSpeed)* _movementParams.timeToReachMaxFallSpeed;
    }

    protected override void OnStateUpdate()
    {

        #region JumpBuffer 
        if(_iWantsJumpWriter.jumpBuffer>0) _iWantsJumpWriter.jumpBuffer -= Time.deltaTime;
        if (_iWantsJumpWriter.wantsJump) _iWantsJumpWriter.jumpBuffer = _movementParams.jumpBuffer;
        #endregion

        if (DetectCollision.isColliding(Vector2.down, StateMachine.transform, Vector2.zero))
        {
                StateMachine.ChangeState(StateMachine.stateIdle);
                return;
        }

        _timer += Time.deltaTime;

        StateMachine.velocity.y = _timer * _movementParams.maxFallSpeed * Vector2.down.y;
        StateMachine.velocity.y = Mathf.Clamp(StateMachine.velocity.y, -_movementParams.maxFallSpeed, _movementParams.maxFallSpeed);

        StateMachine.velocity.x += Time.deltaTime / _movementParams.accelerationTime * _IOrientWriter.orient.x * (_movementParams.apexControl/10);
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -_movementParams.maxSpeed/100, _movementParams.maxSpeed/100);


        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
