using UnityEngine;

public class PlayerStateMachine
{
    public PlayerIdleState IdelState; // 대기 상태

    private PlayerStateBase _currentState; // 현재 상태

    // State 클래스에 필요한 정보를 전달할 컨텍스트를 넘겨줌
    public PlayerStateMachine(PlayerStateContext context)
    {
        IdelState = new PlayerIdleState(context);
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
