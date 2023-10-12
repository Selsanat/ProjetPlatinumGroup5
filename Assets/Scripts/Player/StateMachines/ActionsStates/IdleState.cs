using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DetectCollisionExtension;

public class IdleState : TemplateState
{
    
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        StateMachine.velocity = Vector2.zero;
    }

    protected override void OnStateUpdate()
    {
        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform,Vector3.zero))
        {
            StateMachine.ChangeState(StateMachine.fallState);
            return;
        }

        else
        {
            if (_iWantsJumpWriter.wantsJump || _iWantsJumpWriter.jumpBuffer > 0)
            {
                StateMachine.ChangeState(StateMachine.jumpState);
                return;
            }

            if (_IOrientWriter.orient.x != 0)
            {
                StateMachine.ChangeState(StateMachine.stateAccelerate);
                return;
            }
        }


        StateMachine.velocity = Vector2.zero;
        StateMachine.transform.Translate(StateMachine.velocity*Time.time);
    }
}