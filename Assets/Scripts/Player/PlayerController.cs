using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStateMachine _stateMachine;
    PlayerStateContext _stateContext;

    PlayerInputController _inputController;

    Animator _animator;

    Rigidbody _rigidbody;

    private void Awake()
    {
        InitAnimator();
        InitRigidbody();
        InitInputController();
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
        _animator.applyRootMotion = true;
    }

    private void InitRigidbody()
    {
        TryGetComponent(out _rigidbody);
        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void InitInputController()
    {
        TryGetComponent(out _inputController);
    }

    // 상태 머신 초기화 함수
    private void InitStateMachine()
    {
        // 컨텍스트 초기화
        _stateContext = new PlayerStateContext
        {
            Controller = this,
            InputController = _inputController,
            Animator = _animator,
            Rigidbody = _rigidbody,
            MainCamera = Camera.main
        };

        // 상태 머신 초기화
        _stateMachine = new PlayerStateMachine(_stateContext);

        _stateContext.StateMachine = _stateMachine;

        _stateMachine.InitStateMachine(_stateMachine.IdleState);
    }

    #endregion
}
