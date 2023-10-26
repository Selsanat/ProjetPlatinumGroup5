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


        #region Yvelocity
        float h = _movementParams.jumpMaxHeight;
        float th = _movementParams.fallDuration / 2;
        float g = -(2 * h) / Mathf.Pow(th, 2);

        StateMachine.velocity.y += g * Time.deltaTime;
        StateMachine.velocity.y = Mathf.Clamp(StateMachine.velocity.y, -_movementParams.maxFallSpeed, _movementParams.maxFallSpeed);
        //StateMachine.velocity.y =- _movementParams.gravityScale; 
        #endregion

        #region Xvelocity

        float accelerationTime = _movementParams.fallAccelerationTime;
        float airMaxSpeed = _movementParams.fallMaxSpeedX;

        _timer += Time.deltaTime * _IOrientWriter.orient.x;
        _timer = Mathf.Clamp(_timer, -accelerationTime, accelerationTime);

        StateMachine.velocity.x = Mathf.Abs((_timer / accelerationTime) * airMaxSpeed )* _IOrientWriter.orient.x;
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -airMaxSpeed, airMaxSpeed);

        if (_IOrientWriter.orient.x == 0)
        {
            StateMachine.velocity.x = 0;
        }
        #endregion
        _characterController.Move(StateMachine.velocity);
    }
}
