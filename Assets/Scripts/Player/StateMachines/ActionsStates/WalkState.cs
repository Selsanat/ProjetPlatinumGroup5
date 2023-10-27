using DetectCollisionExtension;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Microsoft.Win32.SafeHandles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class WalkState : TemplateState
{
    
    private CharacterController characterController;
    RaycastHit HitInfo;

    protected override void OnStateInit()
    {
        
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        animator.Play("Walk");
        StateMachine.velocity.x = _movementParams.maxSpeed* Mathf.Sign(StateMachine.velocity.x);
         characterController = StateMachine.GetComponent<CharacterController>();
    }

    protected override void OnStateUpdate()
    {
        #region Death
        if (_iMouvementLockedReader.isMouvementLocked)
        {
            return;
        }
        #endregion
        StateMachine.transform.Translate(StateMachine.velocity); 

        #region Jump

        if (_iWantsJumpWriter.wantsJump || _iWantsJumpWriter.jumpBuffer > 0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        #endregion

        //Debug.DrawRay(origin, Vector2.down * (distance + _movementParams.slideSlopeThresHold), Color.cyan);

        #region Fall
        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, Vector3.zero))
        {
            Vector3 origin = StateMachine.transform.position + characterController.center;
            float distance = characterController.bounds.extents.y + characterController.skinWidth;
            Ray ray = new Ray(origin, Vector2.down);
            Vector3 dir = Vector3.Cross(StateMachine.transform.position, HitInfo.normal);

            if (Physics.Raycast(ray, out HitInfo, (distance + _movementParams.slideSlopeThresHold)))
            {
                dir.z = 0;
                dir *= -_IOrientWriter.orient.x;
                dir = dir.normalized;

                StateMachine.velocity = dir * _movementParams.maxSpeed;
                if (dir.normalized.Abs() == Vector3.right)
                {
                    StateMachine.velocity.y -= 0.1f;
                }

            }
            else
            {
                StateMachine.ChangeState(StateMachine.fallState);
                return;
            }
        }
        else
        {
            StateMachine.velocity.y = 0;
        }
        #endregion

        #region HasHitWall
        /*if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.right, StateMachine.transform, Vector2.zero))
        {
            StateMachine.velocity.x = 0;
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }*/
        #endregion
        #region StopInput
        if (_IOrientWriter.orient.x == 0)
        {
            #region Decelerate
            StateMachine.ChangeState(StateMachine.stateDecelerate);
            return; 
            #endregion
        }
        else
        {
            //Debug.Log(Mathf.Sign(_IOrientWriter.orient.x) + " Mon input, et voila ma velocité : " + Mathf.Sign(StateMachine.velocity.x));
            if (Mathf.Sign(_IOrientWriter.orient.x) != Mathf.Sign(StateMachine.velocity.x))
            {

                StateMachine.ChangeState(StateMachine.turnDecelerateState);
                return;
            }
        }
        #endregion

        _characterController.Move(StateMachine.velocity);


    }
}
