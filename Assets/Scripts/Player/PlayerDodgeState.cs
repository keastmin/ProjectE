using UnityEngine;

public class PlayerDodgeState : PlayerStateBase
{
    private float _dodgeTime = 0f;

    public PlayerDodgeState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Dodge State");

        // ȸ�� �ð��� 0���� ����
        _dodgeTime = 0f;

        // ���� �ӵ��� ȸ�� �ӵ��� ����
        _context.SetCurrentSpeed(_context.Controller.DodgeSpeed);

        // Dodge �ִϸ��̼� Ȱ��ȭ
        _context.AnimatorSetTrigger("IsDodge");
    }

    public override void Execute()
    {
        // ȸ�� ���� �ð� ����
        _dodgeTime += Time.deltaTime;
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
        if(_dodgeTime > _context.Controller.DodgeTime) // ȸ�� ���� �ð��� �� �Ǹ�
        {
            // �޸��� ���·� ��ȯ
            _context.StateMachine.TransitionTo(_context.StateMachine.RunState);
        }
    }
}
