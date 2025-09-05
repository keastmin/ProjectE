using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public PlayerIdleState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Idle State");
    }

    public override void Execute()
    {
        TransitionTo();
    }

    public override void FixedExecute()
    {
        _context.ApplyAnimationVelocity(_context.AniDelta, Time.fixedDeltaTime); // 누적된 deltaPosition으로부터 Velocity 계산 후 적용
        _context.AniDelta = Vector3.zero; // 누적된 값 초기화

    }

    public override void AnimationMoveExecute()
    {
        _context.AniDelta += _context.Animator.deltaPosition; // deltaPosition 누적
    }

    public override void ExitState()
    {
        
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude != 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
    }
}
