using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public PlayerIdleState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Idle State");

        // Idle 애니메이션 활성화
        _context.AnimatorSetBool("IsIdle", true);

        // 현재 속도를 0으로 설정
        _context.SetTargetSpeed(0f);
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {
        // 0까지 도달하는데에 있어 잔여 속도가 있을 수 있기 때문에 Move 진행
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {
        // Idle 애니메이션 비활성화
        _context.AnimatorSetBool("IsIdle", false);
    }

    private void TransitionTo()
    {
        if (_context.MoveInput.sqrMagnitude != 0f)
        {
            _context.StateMachine.TransitionTo(_context.StateMachine.JogState);
        }
        else if (_context.ComboAttackInput) // 콤보 공격 입력이 있을 경우
        {
            // 콤보 공격 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.ComboAttackState);
        }
    }
}
