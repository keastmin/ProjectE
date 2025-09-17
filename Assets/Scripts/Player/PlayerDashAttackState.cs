using UnityEngine;

public class PlayerDashAttackState : PlayerStateBase
{
    public PlayerDashAttackState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        // �̵� �ӵ� �ʱ�ȭ
        _context.SetCurrentSpeed(0f);
        _context.SetTargetSpeed(0f);

        // ������ ������ �÷��� �ʱ�ȭ
        _context.ComboAttackFinishTriggerRevert();

        // �뽬 ���� �ִϸ��̼� Ȱ��ȭ
        _context.AnimatorSetTrigger("IsDashAttack");
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {
        // �ִϸ��̼� ��� ������
        _context.ApplyAnimationVelocity(_context.AniDelta, Time.fixedDeltaTime);
        _context.AniDelta = Vector3.zero;

        TransitionTo();
    }

    public override void AnimationMoveExecute()
    {
        _context.AniDelta += _context.Animator.deltaPosition;
        Debug.Log(_context.AniDelta);
    }

    public override void ExitState()
    {
        _context.ComboAttackFinishTriggerRevert();
    }

    private void TransitionTo()
    {
        if (_context.AnimationEvents.ComboAttackFinish)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
