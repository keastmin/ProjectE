using UnityEngine;

public class PlayerStateMachine
{
    public PlayerIdleState IdelState; // ��� ����

    private PlayerStateBase _currentState; // ���� ����

    // State Ŭ������ �ʿ��� ������ ������ ���ؽ�Ʈ�� �Ѱ���
    public PlayerStateMachine(PlayerStateContext context)
    {
        IdelState = new PlayerIdleState(context);
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
