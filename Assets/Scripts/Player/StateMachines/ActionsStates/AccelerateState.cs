using DetectCollisionExtension;
using Cinemachine.Utility;
using UnityEngine;

public class AccelerateState : TemplateState
{

    private float _timer;
    private CharacterController characterController;
    RaycastHit HitInfo;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = StateMachine.velocity.x / _movementParams.maxSpeed* _movementParams.accelerationTime;
        characterController = StateMachine.GetComponent<CharacterController>();
    }

    protected override void OnStateUpdate()
    {


        _timer += Time.deltaTime;
        #region Jump
        if (_iWantsJumpWriter.wantsJump || _iWantsJumpWriter.jumpBuffer > 0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }
        #endregion

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

                StateMachine.velocity = _timer / _movementParams.accelerationTime*(_movementParams.maxSpeed*dir);
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
            StateMachine.velocity.x = (_timer / _movementParams.accelerationTime) * (_movementParams.maxSpeed);
            StateMachine.velocity.x = Mathf.Abs(StateMachine.velocity.x) * _IOrientWriter.orient.x;
        }
        #endregion


        /*#region HasHitWall
        if (DetectCollision.isColliding(Mathf.Sign(_IOrientWriter.orient.x) * Vector2.right, StateMachine.transform, Vector2.zero))
        {
            StateMachine.velocity.x = 0;
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }
        #endregion*/
        #region StopInput
        if (_IOrientWriter.orient.x == 0)
        {
            #region Decelerate
            StateMachine.ChangeState(StateMachine.stateDecelerate);
            return;
            #endregion
        }
        #endregion

        #region Walk
        //Debug.Log(Mathf.Abs(StateMachine.velocity.x) + " Velocity and Max speed is : " + _movementParams.maxSpeed);
        if (Mathf.Abs(StateMachine.velocity.x) >= _movementParams.maxSpeed)
        {
            StateMachine.transform.Translate(StateMachine.velocity);
            StateMachine.ChangeState(StateMachine.stateWalk);
            return;
        } 
        #endregion


        
        _characterController.Move(StateMachine.velocity);
    }
}
