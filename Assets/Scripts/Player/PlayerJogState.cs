using UnityEngine;

public class PlayerJogState : PlayerStateBase
{
    public PlayerJogState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Jog State");

        // Jog 속도로 목표 속도를 설정
        _context.SetTargetSpeed(_context.Controller.JogSpeed);

        // Move 애니메이션 활성화
        _context.AnimatorSetBool("IsMove", true);
    }

    public override void Execute()
    {

    }

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
        else if (_context.ComboAttackInput) // 콤보 공격 입력이 있을 경우
        {
            // 콤보 공격 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
    }
}
