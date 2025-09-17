using UnityEngine;

public class PlayerStateMachine
{
    public PlayerIdleState IdleState; // 대기 상태
    public PlayerJogState JogState; // 조깅 상태
    public PlayerDodgeState DodgeState; // 회피 상태
    public PlayerRunState RunState; // 달리기 상태
    public PlayerComboAttackState ComboAttackState; // 콤보 공격 상태
    public PlayerDashAttackState DashAttackState; // 대쉬 공격 상태

    private PlayerStateBase _currentState; // 현재 상태
    public PlayerStateBase PrevState;

    // State 클래스에 필요한 정보를 전달할 컨텍스트를 넘겨줌
    public PlayerStateMachine(PlayerStateContext context)
    {
        IdleState = new PlayerIdleState(context);
        JogState = new PlayerJogState(context);
        DodgeState = new PlayerDodgeState(context);
        RunState = new PlayerRunState(context);
        ComboAttackState = new PlayerComboAttackState(context);
        DashAttackState = new PlayerDashAttackState(context);
    }

    // 상태 머신 초기화
    public void InitStateMachine(PlayerStateBase initState)
    {
        _currentState = initState; // 시작 상태 초기화
        _currentState?.EnterState(); // 현재 상태 시작
    }

    // 현재 상태 전환
    public void TransitionTo(PlayerStateBase nextState)
    {
        PrevState = _currentState; // 이전 상태 저장
        _currentState?.ExitState(); // 현재 상태 종료
        _currentState = nextState; // 현재 상태 교체
        _currentState.EnterState(); // 현재 상태 시작
    }

    // Update 반복
    public void Execute()
    {
        _currentState?.Execute();
    }

    // Fixed Update 반복
    public void FixedExcute()
    {
        _currentState?.FixedExecute();
    }

    public void AnimationMoveExtcute()
    {
        _currentState?.AnimationMoveExecute();
    }
}
