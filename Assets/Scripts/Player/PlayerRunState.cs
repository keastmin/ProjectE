using UnityEngine;

public class PlayerRunState : PlayerStateBase
{
    public PlayerRunState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Run State");

        _context.AnimatorSetBool("IsRun", true);
    }

    public override void Execute()
    {
        TransitionTo();
    }

    public override void FixedExecute()
    {
        _context.LookDirection();
        _context.ApplyAnimationVelocity(_context.AniDelta, Time.fixedDeltaTime);
        _context.AniDelta = Vector3.zero;
    }

    public override void AnimationMoveExecute()
    {
        _context.AniDelta += _context.Animator.deltaPosition;
    }

    public override void ExitState()
    {
        _context.AnimatorSetBool("IsRun", false);
    }

    private void TransitionTo()
    {
        if(_context.MoveInput.sqrMagnitude == 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
