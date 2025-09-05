using UnityEngine;

public class PlayerJogState : PlayerStateBase
{
    private Vector3 _accPos = Vector3.zero;

    public PlayerJogState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Jog State");

        // Jog 애니메이션 활성화
        _context.AnimatorSetBool("IsJog", true);
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
        // Jog 애니메이션 비활성화
        _context.AnimatorSetBool("IsJog", false);
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude == 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
