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
        _timer = (StateMachine.velocity.y/_movementParams.maxFallSpeed)* _movementParams.timeToReachMaxFallSpeed;
        Debug.Log(StateMachine.PreviousState);
        Debug.Log(StateMachine.jumpState);
        Debug.Log(StateMachine.PreviousState != StateMachine.jumpState);
        if (StateMachine.PreviousState != StateMachine.jumpState) _coyote = _movementParams.coyoteWindow;
        else _coyote = 0;
    }

    protected override void OnStateUpdate()
    {


        #region JumpBuffer 
        if (_iWantsJumpWriter.jumpBuffer>0) _iWantsJumpWriter.jumpBuffer -= Time.deltaTime;
        if (_iWantsJumpWriter.wantsJump) _iWantsJumpWriter.jumpBuffer = _movementParams.jumpBuffer;
        #endregion

        if (_iWantsJumpWriter.jumpBuffer > 0 && _coyote > 0)
        {
            _iWantsJumpWriter.jumpBuffer = 0;
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        _coyote -= Time.deltaTime;

        if (DetectCollision.isColliding(Vector2.down, StateMachine.transform, StateMachine.velocity))
        {
            
                StateMachine.velocity.y = 0;
                StateMachine.transform.Translate(StateMachine.velocity);
                StateMachine.ChangeState(StateMachine.stateIdle);
                return;

        }

        #region HasHitWall
        if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.right, StateMachine.transform, Vector2.zero))
        {
            StateMachine.velocity.x = 0;
        }
        #endregion

        _timer += Time.deltaTime;

        StateMachine.velocity.y = _timer * _movementParams.maxFallSpeed * Vector2.down.y * _movementParams.gravityScale;
        StateMachine.velocity.y = Mathf.Clamp(StateMachine.velocity.y, -_movementParams.maxFallSpeed, _movementParams.maxFallSpeed);

        StateMachine.velocity.x += Time.deltaTime / _movementParams.accelerationTime * _IOrientWriter.orient.x * (_movementParams.airControl);
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -_movementParams.maxSpeed, _movementParams.maxSpeed);


        StateMachine.transform.Translate(StateMachine.velocity);

    }
}
