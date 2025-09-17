using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _inputMaintainTime = 0.2f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _dodgeAction;
    private InputAction _comboAttackAction;

    // 회피
    public bool DodgeInput;
    private float _dodgeInputTime = 0f;
    private bool _dodgeTrigger = false;

    // 움직입 입력 벡터
    public Vector2 MoveInput;

    // 콤보 공격
    public bool ComboAttackInput;
    private float _comboAttackInputTime = 0f;
    private bool _comboAttackTrigger = false;

    private void Awake()
    {
        InitActions();
        InitValues();
    }

    void Update()
    {
        GetInput();
    }

    private void InitActions()
    {
        TryGetComponent(out _playerInput);
        _moveAction = _playerInput.actions["Move"];
        _dodgeAction = _playerInput.actions["Dodge"];
        _comboAttackAction = _playerInput.actions["ComboAttack"];
    }

    private void InitValues()
    {
        _dodgeInputTime = 0f;
        _dodgeTrigger = false;

       _comboAttackInputTime = 0f;
       _comboAttackTrigger = false;
}

    private void GetInput()
    {
        // 움직임 입력
        MoveInput = _moveAction.ReadValue<Vector2>();

        // 회피 입력
        _dodgeTrigger = _dodgeAction.WasPressedThisFrame();
        if (_dodgeTrigger)
        {
            _dodgeInputTime = 0f;
            DodgeInput = true;
        }
        if (DodgeInput)
        {
            _dodgeInputTime += Time.deltaTime;
            DodgeInput = (_dodgeInputTime < _inputMaintainTime);
        }

        // 콤보 공격 입력
        _comboAttackTrigger = _comboAttackAction.WasPressedThisFrame();
        if (_comboAttackTrigger)
        {
            _comboAttackInputTime = 0f;
            ComboAttackInput = true;
        }
        if (ComboAttackInput)
        {
            _comboAttackInputTime += Time.deltaTime;
            ComboAttackInput = (_comboAttackInputTime < _inputMaintainTime);
        }
    }
}
