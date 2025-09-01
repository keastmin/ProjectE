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

    #region �ʱ�ȭ �Լ�

    private void InitAnimator()
    {
        TryGetComponent(out _animator);
    }

    // ���� �ӽ� �ʱ�ȭ �Լ�
    private void InitStateMachine()
    {
        // ���ؽ�Ʈ �ʱ�ȭ
        _stateContext = new PlayerStateContext
        {
            Controller = this,
            StateMachine = _stateMachine,
            Animator = _animator
        };

        // ���� �ӽ� �ʱ�ȭ
        _stateMachine = new PlayerStateMachine(_stateContext);
        _stateMachine.InitStateMachine(_stateMachine.IdelState);
    }

    #endregion
}
