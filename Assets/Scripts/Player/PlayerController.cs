using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region �ʵ�

    [Header("Movement")]
    [SerializeField] private float _rotateSpeed = 0.2f; // �÷��̾��� ȸ�� �ӵ�
    [SerializeField] private float _speedThreshold = 0.01f;
    [SerializeField] private float _speedLerpValue = 8.0f;
    [SerializeField] private float _jogSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;

    [Header("Dodge")]
    [SerializeField] private float _dodgeTime = 0.8f;
    [SerializeField] private float _dodgeSpeed = 8f;
    [SerializeField] private float _dodgeRotationSpeed = 15f;

    [Header("Combo Attack")]
    [SerializeField] private float _comboAttackRotateTime = 0.1f; // �޺� ���� ���̻��̿� ���� ȸ�� �ð�
    [SerializeField] private float _comboAttackRotateSpeed = 0.4f; // �޺� ���� ���̻��̿� ���� ȸ�� �ӵ�

    // velocity
    private float _currentSpeed = 0f;
    private float _targetSpeed = 0f;

    #endregion

    #region ���¸ӽ�

    // State Machine
    PlayerStateMachine _stateMachine;
    PlayerStateContext _stateContext;

    #endregion

    #region ������Ʈ��

    // Input Controller
    PlayerInputController _inputController;

    // Animator
    Animator _animator;

    // Rigidbody
    Rigidbody _rigidbody;

    // Animation Events
    PlayerAnimationEvents _animationEvnets;

    #endregion

    #region ������Ƽ��

    // rotation
    public float RotateSpeed => _rotateSpeed; // �Ϲ����� ȸ�� �ӵ�
    public float ComboAttackRotateSpeed => _comboAttackRotateSpeed; // �޺� ���� �� ȸ�� �ӵ�
    public float ComboAttackRotateTime => _comboAttackRotateTime; // �޺� ���� ���� �� ȸ�� ���� �ð�

    // velocity
    public float CurrentSpeed // Ÿ�� �ӵ�
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

    public float TargetSpeed // ��ǥ �ӵ�
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
    public float JogSpeed => _jogSpeed; // ���� �ӵ�
    public float RunSpeed => _runSpeed; // �޸��� �ӵ�
    public float DodgeTime => _dodgeTime; // ȸ�� ���� �ð�
    public float DodgeSpeed => _dodgeSpeed; // ȸ�� �ӵ�
    

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
        // ���� �ӵ��� ��ǥ �ӵ��� ����
        SetCurrentSpeed();

        // �ִϸ������� �ӵ� �� ����
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

    // PlayerController�� Update���� ��� �۵��ϴ� �Լ�
    #region ��� �۵� �Լ�

    // ���� �ӵ��� Ÿ�� �ӵ��� ������
    private void SetCurrentSpeed()
    {
        // ���� �ӵ��� ��ǥ �ӵ��� ����
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, _speedLerpValue * Time.deltaTime);

        // ���� �ӵ��� ��ǥ �ӵ��� ����� ��������� ��ǥ �ӵ��� ����
        CurrentSpeed = (Mathf.Abs(CurrentSpeed - TargetSpeed) <= _speedThreshold) ? TargetSpeed : CurrentSpeed;
    }

    // �ִϸ������� �ӵ� �� ����
    private void SetAnimatorFloatValue(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    #endregion

    #region �ʱ�ȭ �Լ�

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

    // ���� �ӽ� �ʱ�ȭ �Լ�
    private void InitStateMachine()
    {
        // ���ؽ�Ʈ �ʱ�ȭ
        _stateContext = new PlayerStateContext
        {
            Controller = this,
            InputController = _inputController,
            Animator = _animator,
            Rigidbody = _rigidbody,
            AnimationEvents = _animationEvnets,
            MainCamera = Camera.main
        };

        // ���� �ӽ� �ʱ�ȭ
        _stateMachine = new PlayerStateMachine(_stateContext);

        _stateContext.StateMachine = _stateMachine;

        _stateMachine.InitStateMachine(_stateMachine.IdleState);
    }

    #endregion
}
