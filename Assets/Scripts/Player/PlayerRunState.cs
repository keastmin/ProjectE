using UnityEngine;

public class PlayerRunState : PlayerStateBase
{
    public PlayerRunState(PlayerStateContext context) : base(context) { }

    // 상태 패턴의 진입
    public override void EnterState()
    {
        Debug.Log("Run State");

        // Run 속도로 목표 속도를 설정
        _context.SetTargetSpeed(_context.Controller.RunSpeed);

        // Move 애니메이션 활성화
        _context.AnimatorSetBool("IsMove", true);
    }

    // 상태 패턴 틱 반복
    public override void Execute()
    {

    }

    // 상태 패턴 물리 틱 반복
    public override void FixedExecute()
    {
        _context.LookDirection(_context.Controller.RotateSpeed);
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {
        // Move 애니메이션 비활성화
        _context.AnimatorSetBool("IsMove", false);
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude == 0f)        // 움직임 입력이 없을 경우
        {
            // Idle 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
        }
        else if (_context.DodgeInput) // 회피 입력이 있을 경우
        {
            // 회피 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.DodgeState);
        }
    }
}
