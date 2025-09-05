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

    // 입력 컨트롤러
    public Vector2 MoveInput => InputController.MoveInput;

    public void AnimatorSetBool(string name, bool value) => Animator.SetBool(name, value);

    // Animator의 deltaPosition을 이용해 Velocity를 반환하는 함수
    public Vector3 GetAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Vector3 dp = deltaPos;

        if (!applyYValue) dp.y = 0f;

        return dp / deltaTime;
    }

    // Animator의 deltaPosition을 이용해 Rigidbody에 Velocity를 적용하는 함수
    public void ApplyAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Rigidbody.linearVelocity = GetAnimationVelocity(deltaPos, deltaTime, applyYValue);
    }

    // Rigidbody에 Velocity를 적용하는 함수
    public void ApplyVelocity(Vector3 velocity)
    {
        Rigidbody.linearVelocity = velocity;
    }

    // 움직임 인풋이 있을 때마다 카메라로 바라보고 있는 방향을 기준으로 입력한 벡터를 방향을 바라보도록 회전하는 함수
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
