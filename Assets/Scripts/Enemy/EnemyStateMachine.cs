using UnityEngine;

public class EnemyStateMachine
{
    public EnemyIdleState IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;
    public EnemyCheckState CheckState;

    private EnemyStateBase _currentState;
    
    public EnemyStateMachine(EnemyController controller)
    {
        IdleState = new EnemyIdleState(controller);
        PatrolState = new EnemyPatrolState(controller);
        ChaseState = new EnemyChaseState(controller);
        CheckState = new EnemyCheckState(controller);
    }

    public void InitState(EnemyStateBase initState)
    {
        _currentState = initState;
        _currentState.EnterState();
    }

    public void TransitionTo(EnemyStateBase nextState)
    {
        _currentState.ExitState();
        _currentState = nextState;
        _currentState.EnterState();
    }

    public void Execute()
    {
        _currentState?.Execute();
    }

    public void FixedExecute()
    {
        _currentState?.FixedExecute();
    }
}
