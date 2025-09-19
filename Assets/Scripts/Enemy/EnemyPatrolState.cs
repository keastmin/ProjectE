using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{
    public EnemyPatrolState(EnemyController controller) : base(controller) { }

    public override void EnterState()
    {

    }

    public override void Execute()
    {

    }

    public override void FixedExecute()
    {
        TransitionTo();
    }

    public override void ExitState()
    {

    }

    private void TransitionTo()
    {

    }
}
