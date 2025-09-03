using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    private Vector3 _accPos;

    public PlayerIdleState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Idle State");
        _accPos = Vector3.zero;
    }

    public override void Execute()
    {
        TransitionTo();
    }

    public override void FixedExecute()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 dp = _accPos;
        dp.y = 0f;

        _accPos = Vector3.zero;

        _context.Rigidbody.linearVelocity = dp / dt;
    }

    public override void AnimationMoveExecute()
    {
        _accPos += _context.Animator.deltaPosition;
    }

    public override void ExitState()
    {
        
    }

    private void TransitionTo()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
    }
}
