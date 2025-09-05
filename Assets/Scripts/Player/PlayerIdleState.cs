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
        _context.ApplyAnimationVelocity(_context.AniDelta, Time.fixedDeltaTime); // ������ deltaPosition���κ��� Velocity ��� �� ����
        _context.AniDelta = Vector3.zero; // ������ �� �ʱ�ȭ

    }

    public override void AnimationMoveExecute()
    {
        _context.AniDelta += _context.Animator.deltaPosition; // deltaPosition ����
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
