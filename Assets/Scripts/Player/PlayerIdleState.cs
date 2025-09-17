using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public PlayerIdleState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Idle State");

        // Idle �ִϸ��̼� Ȱ��ȭ
        _context.AnimatorSetBool("IsIdle", true);

        // ���� �ӵ��� 0���� ����
        _context.SetTargetSpeed(0f);
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {
        // 0���� �����ϴµ��� �־� �ܿ� �ӵ��� ���� �� �ֱ� ������ Move ����
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {
        // Idle �ִϸ��̼� ��Ȱ��ȭ
        _context.AnimatorSetBool("IsIdle", false);
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude != 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
        else if (_context.ComboAttackInput) // �޺� ���� �Է��� ���� ���
        {
            // �޺� ���� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
    }
}
