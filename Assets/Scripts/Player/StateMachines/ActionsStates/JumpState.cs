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
        _timer += Time.deltaTime;
        if (StateMachine.velocity.y < 0)
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }

        #region Yvelocity

        float h;
        float th;
        float g;
        if (_IOrientWriter.orient.y == 0)
        {
            h = _movementParams.minJump;
            th = _movementParams.minJump/ _movementParams.jumpMaxHeight * _movementParams.jumpDuration / 2;
        }
        else
        {
            h = _movementParams.jumpMaxHeight;
            th = _movementParams.jumpDuration / 2;
        }
        g = (-2 * h) / Mathf.Pow(th, 2);

        StateMachine.velocity.y += g * Time.deltaTime; 
        #endregion

        _characterController.Move(StateMachine.velocity);
    }
}
