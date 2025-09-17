using UnityEngine;

public class PlayerComboAttackState : PlayerStateBase
{
    float _rotateTime = 0f;

    public PlayerComboAttackState(PlayerStateContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        _rotateTime = 0f; // 회전 시작 시간 초기화

        _context.SetCurrentSpeed(0f);
        _context.SetTargetSpeed(0f);
        _context.AnimationEvents.NextAttackOff();
        _context.ComboAttackFinishTriggerRevert();
        _context.AnimatorSetTrigger("IsAttack");
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {
        // 일정 시간만 회전
        _rotateTime += Time.deltaTime;
        if(_rotateTime < _context.Controller.ComboAttackRotateTime)
        {
            _context.LookDirection(_context.Controller.ComboAttackRotateSpeed);
        }

        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {
        _context.AnimationEvents.NextAttackOff();
        _context.ComboAttackFinishTriggerRevert();
    }

    private void TransitionTo()
    {
        if (_context.DodgeInput) // 회피 입력이 있으면
        {
            // 회피 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.DodgeState);
        }
        else if(_context.AnimationEvents.CanNextComboAttack && _context.ComboAttackInput) // 다음 콤보 공격으로 전환
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
        else if(_context.AnimationEvents.CanExitComboAttack && _context.MoveInput.sqrMagnitude != 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
        else if (_context.AnimationEvents.ComboAttackFinish) // Idle로 전환
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
