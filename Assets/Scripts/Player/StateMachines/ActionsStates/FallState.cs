using DetectCollisionExtension;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class FallState : TemplateState
{
    private float _timer;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        charAnimator.SetBool("Grounded", false);
        StateMachine.velocity.y = 0;
        if(StateMachine.PreviousState != StateMachine.jumpState)
        StateMachine.CoyoteWindow = _movementParams.CoyoteWindow;
        _timer = StateMachine.velocity.x / _movementParams.fallMaxSpeedX * _movementParams.fallAccelerationTime;
    }

    protected override void OnStateUpdate()
    {
        if ((_iWantsJumpWriter.wantsJump || StateMachine.JumpBuffer > 0) && StateMachine.CoyoteWindow>0)
        {
            StateMachine.ChangeState(StateMachine.jumpState);
            return;
        }

        if (DetectCollision.isColliding(Mathf.Abs(StateMachine.velocity.y) * Vector2.down, StateMachine.transform, Vector2.zero))
        {
            SoundManager.instance.PlayRandomClip("land");
            charAnimator.SetBool("Grounded", true);
            StateMachine.velocity.y = 0;
            if (_IOrientWriter.orient.x != 0 &&
                !DetectCollision.isColliding(Mathf.Sign(_IOrientWriter.orient.x) * Vector2.right, StateMachine.transform, Vector2.zero))
            {
                StateMachine.ChangeState(StateMachine.stateAccelerate);
                return;
            }
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

        StateMachine.velocity.x = Mathf.Abs((_timer / accelerationTime) * airMaxSpeed) * _IOrientWriter.orient.x;
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -airMaxSpeed, airMaxSpeed);

        if (_IOrientWriter.orient.x == 0)
        {
            StateMachine.velocity.x = 0;
        }
        #endregion
        _characterController.Move(StateMachine.velocity);
    }
}
