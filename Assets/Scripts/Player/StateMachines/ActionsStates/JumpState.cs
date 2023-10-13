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
        _iWantsJumpWriter.jumpBuffer = 0;
    }

    protected override void OnStateUpdate()
    {
        _timer += Time.deltaTime;
        StateMachine.velocity.y = _movementParams.jumpHeight * (1 - _timer / _movementParams.timeToReachJumpHeight);
        (_IOrientWriter.orient.y == 0 && _timer)
        //Debug.Log(StateMachine.velocity.y);





        if (_movementParams.timeToReachJumpHeight-_timer <= _movementParams.apexModifier)
        {
            //Debug.Log("Apex");
            StateMachine.velocity.y = 0;
            StateMachine.velocity.x = _movementParams.maxSpeed * _IOrientWriter.orient.x * (_movementParams.apexControl);
            StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -_movementParams.maxSpeed, _movementParams.maxSpeed);
        }
        else
        {
            //Debug.Log("Not Apex");
            StateMachine.velocity.x += Time.deltaTime / _movementParams.accelerationTime * _IOrientWriter.orient.x * (_movementParams.airControl);
            StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -_movementParams.maxSpeed, _movementParams.maxSpeed);
        }

        StateMachine.transform.Translate(StateMachine.velocity);


        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, StateMachine.velocity)&& _timer>=_movementParams.timeToReachJumpHeight 
            || DetectCollision.isColliding(Vector2.up, StateMachine.transform, StateMachine.velocity)
            )
        {
                ChangeState(StateMachine.fallState);
                return;
        }

        #region HasHitWall
        if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.right, StateMachine.transform, Vector2.zero))
        {
            StateMachine.velocity.x = 0;
        }
        #endregion


    }
}
