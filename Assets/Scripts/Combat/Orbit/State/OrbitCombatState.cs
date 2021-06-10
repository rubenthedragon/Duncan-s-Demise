public class OrbitCombatState : OrbitState
{
    private Orbit orbit;
    private CombatItemObject combatItem;
    private Direction direction;

    public OrbitCombatState(Orbit orbit, Direction dir) : base(orbit)
    {
        this.orbit = orbit;
        combatItem = orbit.CurrentCombatItemObject;
        direction = dir;
    }

    public override void PrepareState()
    {
        if (!combatItem.EnoughMana(combatItem.GetUsedAbility(direction)))
        {
            orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
            return;
        }

        ObjectActivator.Deactivate(orbit.DefaultArrow);
        ObjectActivator.Activate(combatItem.gameObject);

        combatItem.transform.parent = orbit.FocusPointer.transform;
        combatItem.transform.position = orbit.FocusPointer.transform.position;
        combatItem.transform.rotation = orbit.FocusPointer.transform.rotation;

        combatItem.UseAbility(orbit, direction);
    }

}