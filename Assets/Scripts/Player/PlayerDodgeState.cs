using UnityEngine;

public class PlayerDodgeState : PlayerStateBase
{
    private float _dodgeTime = 0f;
    private bool _moveDodge = false;

    public PlayerDodgeState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        // ������ �Է��� �ִ� ���¿��� ������ ������ �Ǻ�
        _moveDodge = _context.MoveInput.sqrMagnitude > 0f;

        Debug.Log("Dodge State");

        // ȸ�� �ð��� 0���� ����
        _dodgeTime = 0f;

        // Dodge �ִϸ��̼� Ȱ��ȭ
        if (_moveDodge)
        {
            // ��� �Է��� �������� ȸ��
            _context.LookDirection(1f);

            // ���� �ӵ��� ȸ�� �ӵ��� ����
            _context.SetCurrentSpeed(_context.Controller.DodgeSpeed);
            _context.AnimatorSetTrigger("IsDodge");
        }
        else
        {
            // ���� �ӵ��� ȸ�� �ӵ��� ���������� �ڷ� ������ ����
            _context.SetCurrentSpeed(_context.Controller.DodgeSpeed * -1);
            _context.AnimatorSetTrigger("IsBackDodge");
        }
    }

    public override void Execute()
    {

    }

    public override void FixedExecute()
    {
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {

    }

    private void TransitionTo()
    {
        // ȸ�� ���� �ð� ����
        _dodgeTime += Time.fixedDeltaTime;

        if (_dodgeTime > _context.Controller.DodgeTime) // ȸ�� ���� �ð��� �� �Ǹ�
        {
            if (_moveDodge)
            {
                // �޸��� ���·� ��ȯ
                _context.StateMachine.TransitionTo(_context.StateMachine.RunState);
            }
            else
            {
                // Idle ���·� ��ȯ
                _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
            }
        }
        else if (_context.ComboAttackInput && _moveDodge) // ���� �Է��� �ְ� ������ ȸ�ǿ��� ���
        {
            // �뽬 ���� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.DashAttackState);
        }
    }
}
