using UnityEngine;

public abstract class PlayerStateBase
{
    protected PlayerStateContext _context;

    public PlayerStateBase(PlayerStateContext context)
    {
        _context = context;
    }

    public abstract void EnterState();
    public abstract void Execute();
    public abstract void FixedExecute();
    public abstract void ExitState();

    public virtual void AnimationMoveExecute()
    {
        
    }
}
