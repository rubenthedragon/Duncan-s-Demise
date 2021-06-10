
public abstract class EnemyState
{
    public EnemyState(Enemy enemy) { }

    public abstract void ExecuteState();
    public virtual void ExecuteFixedState() { }
    public virtual void PrepareState() { }
    public virtual void ExitState() { }
}