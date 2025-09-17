using UnityEngine;

public sealed class PlayerStateContext
{
    public PlayerController Controller;
    public PlayerStateMachine StateMachine;
    public PlayerInputController InputController;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public PlayerAnimationEvents AnimationEvents;
    public Camera MainCamera;

    public Vector3 AniDelta = Vector3.zero;

    // �Է� ��Ʈ�ѷ�
    public Vector2 MoveInput => InputController.MoveInput;
    public bool DodgeInput => InputController.DodgeInput;
    public bool ComboAttackInput => InputController.ComboAttackInput;

    // �ִϸ��̼� ������Ƽ �� ����
    public void AnimatorSetBool(string name, bool value) => Animator.SetBool(name, value);
    public void AnimatorSetFloat(string name, float value) => Animator.SetFloat(name, value);
    public void AnimatorSetTrigger(string name) => Animator.SetTrigger(name);


    // Animator�� deltaPosition�� �̿��� Rigidbody�� Velocity�� �����ϴ� �Լ�
    public void ApplyAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Vector3 velocity = GetAnimationVelocity(deltaPos, deltaTime, applyYValue);
        ApplyVelocity(velocity);
    }

    // Animator�� deltaPosition�� �̿��� Velocity�� ��ȯ�ϴ� �Լ�
    public Vector3 GetAnimationVelocity(Vector3 deltaPos, float deltaTime, bool applyYValue = false)
    {
        Vector3 dp = deltaPos;

        if (!applyYValue) dp.y = 0f;

        return dp / deltaTime;
    }

    // Rigidbody�� Velocity�� �����ϴ� �Լ�
    public void ApplyVelocity(Vector3 velocity)
    {
        Rigidbody.linearVelocity = velocity;
    }

    // ��ǥ �ӵ��� �����ϴ� �Լ�
    public void SetTargetSpeed(float speed)
    {
        Controller.TargetSpeed = speed;
    }

    // ���� �ӵ��� �����ϴ� �Լ�(���� x)
    public void SetCurrentSpeed(float speed)
    {
        SetTargetSpeed(speed);
        Controller.CurrentSpeed = speed;
    }

    // �÷��̾ ������ �� ����ϴ� �Լ�
    public void Move()
    {
        Vector3 forward = Controller.transform.forward;
        ApplyVelocity(forward * Controller.CurrentSpeed);
    }

    // ������ ��ǲ�� ���� ������ ī�޶�� �ٶ󺸰� �ִ� ������ �������� �Է��� ���͸� ������ �ٶ󺸵��� ȸ���ϴ� �Լ�
    public void LookDirection(float rotationSpeed)
    {
        if (MoveInput != Vector2.zero)
        {
            Vector3 moveDir = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized;
            Vector3 cameraForward = MainCamera.transform.forward;
            cameraForward.y = 0f;
            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
            Vector3 lookDir = cameraRotation * moveDir;
            Controller.transform.rotation = Quaternion.Slerp(Controller.transform.rotation, Quaternion.LookRotation(lookDir), rotationSpeed);
        }
    }

    // �޺� ������ ���� Ʈ���Ÿ� ���󺹱� ��Ű�� �Լ�
    public void ComboAttackFinishTriggerRevert()
    {
        AnimationEvents.ComboAttackFinish = false;
        AnimationEvents.CanExitComboAttack = false;
    }
}
