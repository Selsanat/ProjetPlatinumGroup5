using Cinemachine.Utility;
using DetectCollisionExtension;
using UnityEngine;

public class TurnDecelerateState : TemplateState
{
    private float sign;
    private float _timer;
    private CharacterController characterController;
    RaycastHit HitInfo;

    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        _timer = (1 - (Mathf.Abs(StateMachine.velocity.x / _movementParams.maxSpeed))) * _movementParams.turnDecelerationTime;
        sign = Mathf.Sign(StateMachine.velocity.x);
        characterController = StateMachine.GetComponent<CharacterController>();

    }

    protected override void OnStateUpdate()
    {
        if (StateMachine._iMouvementLockedReader.isMouvementLocked) return;
        _timer += Time.deltaTime;
        #region Jump
        if (_iWantsJumpWriter.wantsJump || StateMachine.JumpBuffer > 0)
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

            if (Physics.Raycast(ray, out HitInfo, (distance + _movementParams.slideSlopeThresHold), ~LayerMask.GetMask("boule") + LayerMask.GetMask("Player")))
            {
                dir.z = 0;
                dir *= -_IOrientWriter.orient.x;
                dir = dir.normalized;

                StateMachine.velocity = _timer / _movementParams.turnDecelerationTime * (_movementParams.maxSpeed * dir);
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
            StateMachine.velocity.x = (_movementParams.turnDecelerationTime - _timer) * (_movementParams.maxSpeed * Vector2.right.x * sign);
            StateMachine.velocity.x = Mathf.Abs(StateMachine.velocity.x) * _IOrientWriter.orient.x;
        }
        #endregion
        #region HasHitWall
        if (DetectCollision.isColliding(Mathf.Sign(StateMachine.velocity.x) * Vector2.right, StateMachine.transform, Vector2.zero))
        {
            StateMachine.velocity.x = 0;
            StateMachine.ChangeState(StateMachine.stateIdle);
            return;
        }
        #endregion
        #region StopInput
        if (_IOrientWriter.orient.x == 0)
        {
            #region ToIdle

            if (_timer >= _movementParams.turnDecelerationTime)
            {
                StateMachine.ChangeState(StateMachine.stateIdle);
                return;
            }
            #endregion
        }
        else
        {
            if (_timer >= _movementParams.turnDecelerationTime)
            {
                StateMachine.ChangeState(StateMachine.stateTurnAccelerateState);
                return;
            }
        }
        #endregion


        //Debug.Log(_timer + " Temps atm, et temps de deceleration : " + _movementParams.turnDecelerationTime);

        StateMachine.transform.Translate(StateMachine.velocity);
    }
}
