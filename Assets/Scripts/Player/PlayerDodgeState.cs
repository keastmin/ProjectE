using UnityEngine;

public class PlayerDodgeState : PlayerStateBase
{
    private float _dodgeTime = 0f;
    private bool _moveDodge = false;

    public PlayerDodgeState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        // 움직임 입력이 있는 상태에서 진입한 것인지 판별
        _moveDodge = _context.MoveInput.sqrMagnitude > 0f;

        Debug.Log("Dodge State");

        // 회피 시간을 0으로 설정
        _dodgeTime = 0f;

        // Dodge 애니메이션 활성화
        if (_moveDodge)
        {
            // 즉시 입력한 방향으로 회전
            _context.LookDirection(1f);

            // 현재 속도를 회피 속도로 설정
            _context.SetCurrentSpeed(_context.Controller.DodgeSpeed);
            _context.AnimatorSetTrigger("IsDodge");
        }
        else
        {
            // 현재 속도를 회피 속도로 설정하지만 뒤로 가도록 설정
            _context.SetCurrentSpeed(_context.Controller.DodgeSpeed * -1);
            _context.AnimatorSetTrigger("IsBackDodge");
        }
    }

    public override void Execute()
    {

    }

    public override void FixedExecute()
    {
        _context.Move();
        TransitionTo();
    }

    public override void ExitState()
    {

    }

    private void TransitionTo()
    {
        // 회피 유지 시간 누적
        _dodgeTime += Time.fixedDeltaTime;

        if (_dodgeTime > _context.Controller.DodgeTime) // 회피 유지 시간이 다 되면
        {
            if (_moveDodge)
            {
                // 달리기 상태로 전환
                _context.StateMachine.TransitionTo(_context.StateMachine.RunState);
            }
            else
            {
                // Idle 상태로 전환
                _context.StateMachine.TransitionTo(_context.StateMachine.IdleState);
            }
        }
        else if (_context.ComboAttackInput && _moveDodge) // 공격 입력이 있고 움직임 회피였을 경우
        {
            // 대쉬 공격 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.DashAttackState);
        }
    }
}
