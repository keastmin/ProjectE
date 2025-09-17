using UnityEngine;

public class PlayerDodgeState : PlayerStateBase
{
    private float _dodgeTime = 0f;

    public PlayerDodgeState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Dodge State");

        // 회피 시간을 0으로 설정
        _dodgeTime = 0f;

        // 현재 속도를 회피 속도로 설정
        _context.SetCurrentSpeed(_context.Controller.DodgeSpeed);

        // Dodge 애니메이션 활성화
        _context.AnimatorSetTrigger("IsDodge");
    }

    public override void Execute()
    {
        // 회피 유지 시간 누적
        _dodgeTime += Time.deltaTime;
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
        if(_dodgeTime > _context.Controller.DodgeTime) // 회피 유지 시간이 다 되면
        {
            // 달리기 상태로 전환
            _context.StateMachine.TransitionTo(_context.StateMachine.RunState);
        }
    }
}
