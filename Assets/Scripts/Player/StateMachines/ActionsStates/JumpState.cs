using DetectCollisionExtension;
using UnityEngine;

public class JumpState : TemplateState
{
    private float _timer;
    protected override void OnStateInit()
    {

    }

    protected override void OnStateEnter(TemplateState previousState)
    {
        charAnimator.SetTrigger("Jump");
        charAnimator.SetBool("Grounded", false);
        SoundManager.instance.PlayRandomClip("Jump");
        _timer = StateMachine.velocity.x / _movementParams.airMaxSpeedX * _movementParams.JumpAccelerationTime;

        float h = _movementParams.jumpMaxHeight;
        float th = _movementParams.jumpDuration / 2;

        StateMachine.velocity.x *= _movementParams.inertieLoss;
        StateMachine.velocity.y = 2 * h / th;
        StateMachine.JumpBuffer = 0;
        StateMachine.CoyoteWindow = 0;

    }

    protected override void OnStateUpdate()
    {
        if ((StateMachine.velocity.y < 0 || DetectCollision.isColliding(Vector2.up, StateMachine.transform, Vector3.zero, false)) ||StateMachine._iMouvementLockedReader.isMouvementLocked)
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
            th = _movementParams.minJump / _movementParams.jumpMaxHeight * _movementParams.jumpDuration / 2;
        }
        else
        {
            h = _movementParams.jumpMaxHeight;
            th = _movementParams.jumpDuration / 2;
        }
        g = (-2 * h) / Mathf.Pow(th, 2);

        StateMachine.velocity.y += g * Time.deltaTime;
        #endregion

        #region Xvelocity

        float accelerationTime = _movementParams.JumpAccelerationTime;
        float airMaxSpeed = _movementParams.airMaxSpeedX;

        _timer += Time.deltaTime * _IOrientWriter.orient.x;
        _timer = Mathf.Clamp(_timer, -accelerationTime, accelerationTime);

        StateMachine.velocity.x = (_timer / accelerationTime) * airMaxSpeed;
        StateMachine.velocity.x = Mathf.Clamp(StateMachine.velocity.x, -airMaxSpeed, airMaxSpeed);
        #endregion

        _characterController.Move(StateMachine.velocity);
    }
}
