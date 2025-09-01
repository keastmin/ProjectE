using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStateMachine _stateMachine;
    PlayerStateContext _stateContext;

    Animator _animator;

    private void Awake()
    {
        InitAnimator();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        _stateMachine.Execute();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }

    private void OnAnimatorMove()
    {
        _stateMachine.AnimationMoveExtcute();
    }

    #region 초기화 함수

    private void InitAnimator()
    {
        TryGetComponent(out _animator);
    }

    // 상태 머신 초기화 함수
    private void InitStateMachine()
    {
        // 컨텍스트 초기화
        _stateContext = new PlayerStateContext
        {
            Controller = this,
            StateMachine = _stateMachine,
            Animator = _animator
        };

        // 상태 머신 초기화
        _stateMachine = new PlayerStateMachine(_stateContext);
        _stateMachine.InitStateMachine(_stateMachine.IdelState);
    }

    #endregion
}
