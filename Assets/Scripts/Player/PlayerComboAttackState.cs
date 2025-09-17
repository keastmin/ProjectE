using UnityEngine;

public class PlayerComboAttackState : PlayerStateBase
{
    float _rotateTime = 0f;

    public PlayerComboAttackState(PlayerStateContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        _rotateTime = 0f; // ȸ�� ���� �ð� �ʱ�ȭ

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
        // ���� �ð��� ȸ��
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
        if (_context.DodgeInput) // ȸ�� �Է��� ������
        {
            // ȸ�� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.DodgeState);
        }
        else if(_context.AnimationEvents.CanNextComboAttack && _context.ComboAttackInput) // ���� �޺� �������� ��ȯ
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
        else if(_context.AnimationEvents.CanExitComboAttack && _context.MoveInput.sqrMagnitude != 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
        else if (_context.AnimationEvents.ComboAttackFinish) // Idle�� ��ȯ
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
