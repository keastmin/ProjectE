using UnityEngine;

public class PlayerJogState : PlayerStateBase
{
    public PlayerJogState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Jog State");

        // Jog �ӵ��� ��ǥ �ӵ��� ����
        _context.SetTargetSpeed(_context.Controller.JogSpeed);

        // Move �ִϸ��̼� Ȱ��ȭ
        _context.AnimatorSetBool("IsMove", true);
    }

    public override void Execute()
    {

    }

    public override void FixedExecute()
    {
        _context.LookDirection(_context.Controller.RotateSpeed);
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {
        // Move �ִϸ��̼� ��Ȱ��ȭ
        _context.AnimatorSetBool("IsMove", false);
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude == 0f)        // ������ �Է��� ���� ���
        {
            // Idle ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
        else if (_context.DodgeInput) // ȸ�� �Է��� ���� ���
        {
            // ȸ�� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.DodgeState);
        }
        else if (_context.ComboAttackInput) // �޺� ���� �Է��� ���� ���
        {
            // �޺� ���� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
    }
}
