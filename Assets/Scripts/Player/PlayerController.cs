using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 필드

    [Header("Movement")]
    [SerializeField] private float _rotateSpeed = 0.2f; // 플레이어의 회전 속도
    [SerializeField] private float _speedThreshold = 0.01f;
    [SerializeField] private float _speedLerpValue = 8.0f;
    [SerializeField] private float _jogSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;

    [Header("Dodge")]
    [SerializeField] private float _dodgeTime = 0.8f;
    [SerializeField] private float _dodgeSpeed = 8f;
    [SerializeField] private float _dodgeRotationSpeed = 15f;

    [Header("Combo Attack")]
    [SerializeField] private float _comboAttackRotateTime = 0.1f; // 콤보 공격 사이사이에 들어가는 회전 시간
    [SerializeField] private float _comboAttackRotateSpeed = 0.4f; // 콤보 공격 사이사이에 들어가는 회전 속도

    // velocity
    private float _currentSpeed = 0f;
    private float _targetSpeed = 0f;

    #endregion

    #region 상태머신

    // State Machine
    PlayerStateMachine _stateMachine;
    PlayerStateContext _stateContext;

    #endregion

    #region 컴포넌트들

    // Input Controller
    PlayerInputController _inputController;

    // Animator
    Animator _animator;

    // Rigidbody
    Rigidbody _rigidbody;

    // Animation Events
    PlayerAnimationEvents _animationEvnets;

    #endregion

    #region 프로퍼티들

    // rotation
    public float RotateSpeed => _rotateSpeed; // 일반적인 회전 속도
    public float ComboAttackRotateSpeed => _comboAttackRotateSpeed; // 콤보 공격 중 회전 속도
    public float ComboAttackRotateTime => _comboAttackRotateTime; // 콤보 공격 시작 후 회전 지속 시간

    // velocity
    public float CurrentSpeed // 타겟 속도
    {
        get
        {
            return _currentSpeed;
        }
        set
        {
            _currentSpeed = value;
        }
    }

    public float TargetSpeed // 목표 속도
    {
        get
        {
            return _targetSpeed;
        }
        set
        {
            _targetSpeed = value;
        }
    }
    public float JogSpeed => _jogSpeed; // 조깅 속도
    public float RunSpeed => _runSpeed; // 달리기 속도
    public float DodgeTime => _dodgeTime; // 회피 유지 시간
    public float DodgeSpeed => _dodgeSpeed; // 회피 속도
    

    #endregion

    private void Awake()
    {
        InitVelocity();
        InitAnimator();
        InitRigidbody();
        InitInputController();
        InitAnimationEvents();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        // 현재 속도를 목표 속도로 보간
        SetCurrentSpeed();

        // 애니메이터의 속도 값 적용
        SetAnimatorFloatValue("Speed", CurrentSpeed);

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

    // PlayerController의 Update에서 상시 작동하는 함수
    #region 상시 작동 함수

    // 현재 속도를 타겟 속도로 보간함
    private void SetCurrentSpeed()
    {
        // 현재 속도를 목표 속도로 보간
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, _speedLerpValue * Time.deltaTime);

        // 현재 속도가 목표 속도에 충분히 가까워지면 목표 속도로 설정
        CurrentSpeed = (Mathf.Abs(CurrentSpeed - TargetSpeed) <= _speedThreshold) ? TargetSpeed : CurrentSpeed;
    }

    // 애니메이터의 속도 값 적용
    private void SetAnimatorFloatValue(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    #endregion

    #region 초기화 함수

    private void InitVelocity()
    {
        CurrentSpeed = 0f;
        TargetSpeed = 0f;
    }

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

    private void InitAnimationEvents()
    {
        TryGetComponent(out _animationEvnets);
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
            AnimationEvents = _animationEvnets,
            MainCamera = Camera.main
        };

        // 상태 머신 초기화
        _stateMachine = new PlayerStateMachine(_stateContext);

        _stateContext.StateMachine = _stateMachine;

        _stateMachine.InitStateMachine(_stateMachine.IdleState);
    }

    #endregion
}
