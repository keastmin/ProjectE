using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    // ������ �Է� ����
    public Vector2 MoveInput;

    private void Awake()
    {
        TryGetComponent(out _playerInput);
        _moveAction = _playerInput.actions["Move"];
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
    }
}
