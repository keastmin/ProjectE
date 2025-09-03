using UnityEngine;

public class PlayerJogState : PlayerStateBase
{
    private Vector3 _accPos = Vector3.zero;

    public PlayerJogState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Jog State");
        _context.Animator.SetBool("IsJog", true);
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
        _context.Animator.SetBool("IsJog", false);
    }

    private void TransitionTo()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
    }
}
