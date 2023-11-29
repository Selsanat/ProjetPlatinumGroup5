using DetectCollisionExtension;
using Cinemachine.Utility;
using UnityEngine;

public class WalkState : TemplateState
{
    
    private CharacterController characterController;
    RaycastHit HitInfo;
    private float distanceGround;
    protected override void OnStateInit()
    {
        
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        SoundManager.instance.PlayClip("Step");
        StateMachine.velocity.x = _movementParams.maxSpeed* Mathf.Sign(StateMachine.velocity.x);
         characterController = StateMachine.GetComponent<CharacterController>();
    }

    protected override void OnStateUpdate()
    {
        if (StateMachine._iMouvementLockedReader.isMouvementLocked) return;
        #region Death
        if (StateMachine._iMouvementLockedReader.isMouvementLocked)
        {
            return;
        }
        #endregion
        StateMachine.transform.Translate(StateMachine.velocity); 

        #region Jump

        if (_iWantsJumpWriter.wantsJump || StateMachine.JumpBuffer > 0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        #endregion

        //Debug.DrawRay(origin, Vector2.down * (distance + _movementParams.slideSlopeThresHold), Color.cyan);

        #region Fall
        if (!DetectCollision.isColliding(Vector2.down, StateMachine.transform, Vector3.zero))
        {
            if(Mathf.Sign(_IOrientWriter.orient.x) != Mathf.Sign(StateMachine.velocity.x))
            {
                distanceGround = 0;
                StateMachine.ChangeState(StateMachine.fallState);
                return;
            }
            Vector3 newCenter = characterController.center;
            newCenter.x += characterController.radius* -Mathf.Sign(StateMachine.velocity.x);
            Vector3 origin = StateMachine.transform.position + newCenter;
            float distance = characterController.bounds.extents.y + characterController.skinWidth;
            Ray ray = new Ray(origin, Vector2.down);
            Vector3 dir = Vector3.Cross(-StateMachine.transform.forward* _IOrientWriter.orient.x, HitInfo.normal);
            Debug.Log(dir);
            Debug.DrawRay(origin, dir, Color.yellow);
            Debug.DrawRay(origin, HitInfo.normal, Color.magenta);
            Debug.DrawRay(origin, Vector2.down * (distance + _movementParams.slideSlopeThresHold), Color.cyan);
            if (Physics.Raycast(ray, out HitInfo, (distance + _movementParams.slideSlopeThresHold),~LayerMask.GetMask("boule") + LayerMask.GetMask("Player")))
            {
                if (Mathf.Abs(dir.x) == 1)
                {
                    Debug.Log("Descend");
                    StateMachine.velocity.y -= 0.1f;
                }
                if (distanceGround == 0)
                {
                    distanceGround = HitInfo.distance;
                }
                if (distanceGround < HitInfo.distance && dir!=Vector3.zero)
                {
                    dir.z = 0;
                    dir.x = Mathf.Abs(dir.x)*_IOrientWriter.orient.x;
                    dir = dir.normalized;
                    StateMachine.velocity = dir * _movementParams.maxSpeed*_movementParams.SpeedBoostOnSlope;
                }
            }
            else
            {
                distanceGround = 0;
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
            //Debug.Log(Mathf.Sign(_IOrientWriter.orient.x) + " Mon input, et voila ma velocit� : " + Mathf.Sign(StateMachine.velocity.x));
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
