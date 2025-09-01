using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public PlayerIdleState(PlayerStateContext context) : base(context) { }

    public override void EnterState()
    {
        Debug.Log("Idle State");
    }

    public override void Execute()
    {
        
    }

    public override void FixedExecute()
    {

    }

    public override void AnimationMoveExecute()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
