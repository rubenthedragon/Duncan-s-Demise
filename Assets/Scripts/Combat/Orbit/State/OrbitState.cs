
public abstract class OrbitState 
{
    public OrbitState(Orbit orbit) { }

    public virtual void ExecuteState() { }
    public virtual void PrepareState() { }
    public virtual void ExitState() { }
}