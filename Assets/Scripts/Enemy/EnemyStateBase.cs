using UnityEngine;

public abstract class EnemyStateBase
{
    protected EnemyController _controller;

    public EnemyStateBase(EnemyController controller)
    {
        _controller = controller;
    }

    public abstract void EnterState();
    public abstract void Execute();
    public abstract void FixedExecute();
    public abstract void ExitState();
}
