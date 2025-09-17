using UnityEngine;

public class PlayerDashAttackState : PlayerStateBase
{
    public PlayerDashAttackState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        // 이동 속도 초기화
        _context.SetCurrentSpeed(0f);
        _context.SetTargetSpeed(0f);

        // 공격이 끝나는 플래그 초기화
        _context.ComboAttackFinishTriggerRevert();

        // 대쉬 공격 애니메이션 활성화
        _context.AnimatorSetTrigger("IsDashAttack");
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {
        // 애니메이션 기반 움직임
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
