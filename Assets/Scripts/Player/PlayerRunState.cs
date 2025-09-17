using UnityEngine;

public class PlayerRunState : PlayerStateBase
{
    public PlayerRunState(PlayerStateContext context) : base(context) { }

    // ���� ������ ����
    public override void EnterState()
    {
        Debug.Log("Run State");

        // Run �ӵ��� ��ǥ �ӵ��� ����
        _context.SetTargetSpeed(_context.Controller.RunSpeed);

        // Move �ִϸ��̼� Ȱ��ȭ
        _context.AnimatorSetBool("IsMove", true);
    }

    // ���� ���� ƽ �ݺ�
    public override void Execute()
    {

    }

    // ���� ���� ���� ƽ �ݺ�
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
    }
}
