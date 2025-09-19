using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float _idleTime = 0f;
    private float _randIdleTime = 0f;

    public EnemyIdleState(EnemyController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Enemy Enter Idle State");

        _idleTime = 0f;
        _randIdleTime = _controller.RandIdleTime;
    }

    public override void Execute()
    {
        _idleTime += Time.deltaTime;

        TransitionTo();
    }

    public override void FixedExecute()
    {
        
    }

    public override void ExitState()
    {
        
    }

    private void TransitionTo()
    {
        if(_idleTime > _randIdleTime)
        {
            _controller.StateMachine.TransitionTo(_controller.StateMachine.PatrolState);
        }
    }
}
