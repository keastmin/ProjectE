using UnityEngine;

public class PlayerStateMachine
{
    public PlayerIdleState IdleState; // ��� ����
    public PlayerJogState JogState; // ���� ����
    public PlayerDodgeState DodgeState; // ȸ�� ����
    public PlayerRunState RunState; // �޸��� ����
    public PlayerComboAttackState ComboAttackState; // �޺� ���� ����
    public PlayerDashAttackState DashAttackState; // �뽬 ���� ����

    private PlayerStateBase _currentState; // ���� ����
    public PlayerStateBase PrevState;

    // State Ŭ������ �ʿ��� ������ ������ ���ؽ�Ʈ�� �Ѱ���
    public PlayerStateMachine(PlayerStateContext context)
    {
        IdleState = new PlayerIdleState(context);
        JogState = new PlayerJogState(context);
        DodgeState = new PlayerDodgeState(context);
        RunState = new PlayerRunState(context);
        ComboAttackState = new PlayerComboAttackState(context);
        DashAttackState = new PlayerDashAttackState(context);
    }

    // ���� �ӽ� �ʱ�ȭ
    public void InitStateMachine(PlayerStateBase initState)
    {
        _currentState = initState; // ���� ���� �ʱ�ȭ
        _currentState?.EnterState(); // ���� ���� ����
    }

    // ���� ���� ��ȯ
    public void TransitionTo(PlayerStateBase nextState)
    {
        PrevState = _currentState; // ���� ���� ����
        _currentState?.ExitState(); // ���� ���� ����
        _currentState = nextState; // ���� ���� ��ü
        _currentState.EnterState(); // ���� ���� ����
    }

    // Update �ݺ�
    public void Execute()
    {
        _currentState?.Execute();
    }

    // Fixed Update �ݺ�
    public void FixedExcute()
    {
        _currentState?.FixedExecute();
    }

    public void AnimationMoveExtcute()
    {
        _currentState?.AnimationMoveExecute();
    }
}
