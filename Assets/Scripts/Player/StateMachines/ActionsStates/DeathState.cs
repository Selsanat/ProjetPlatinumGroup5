using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeathState : TemplateState
{
    protected override void OnStateInit()
    {
    }


    protected override void OnStateEnter(TemplateState previousState)
    {
        if(previousState == StateMachine.jumpState || previousState == StateMachine.fallState)
        {
            StateMachine.velocity.x = 0;
            StateMachine.velocity.y = 0;
        }
        else
            StateMachine.velocity.x = 0;

        //play death animation


    }
    protected override void OnStateUpdate()
    {

    }
}
