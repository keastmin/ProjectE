using UnityEngine;

public sealed class PlayerStateContext
{
    public PlayerController Controller;
    public PlayerStateMachine StateMachine;
    public PlayerInputController InputController;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public Camera MainCamera;

    public Vector3 AniDelta = Vector3.zero;

    // �Է� ��Ʈ�ѷ�
    public Vector2 MoveInput => InputController.MoveInput;

    public void AnimatorSetBool(string name, bool value) => Animator.SetBool(name, value);

    // Animator�� deltaPosition�� �̿��� Velocity�� ��ȯ�ϴ� �Լ�
    public Vector3 GetAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Vector3 dp = deltaPos;

        if (!applyYValue) dp.y = 0f;

        return dp / deltaTime;
    }

    // Animator�� deltaPosition�� �̿��� Rigidbody�� Velocity�� �����ϴ� �Լ�
    public void ApplyAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Rigidbody.linearVelocity = GetAnimationVelocity(deltaPos, deltaTime, applyYValue);
    }

    // Rigidbody�� Velocity�� �����ϴ� �Լ�
    public void ApplyVelocity(Vector3 velocity)
    {
        Rigidbody.linearVelocity = velocity;
    }

    // ������ ��ǲ�� ���� ������ ī�޶�� �ٶ󺸰� �ִ� ������ �������� �Է��� ���͸� ������ �ٶ󺸵��� ȸ���ϴ� �Լ�
    public void LookDirection()
    {
        if (MoveInput != Vector2.zero)
        {
            Vector3 moveDir = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized;
            Vector3 cameraForward = MainCamera.transform.forward;
            cameraForward.y = 0f;
            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
            Vector3 lookDir = cameraRotation * moveDir;
            Controller.transform.rotation = Quaternion.Slerp(Controller.transform.rotation, Quaternion.LookRotation(lookDir), 0.3f);
        }
    }
}
