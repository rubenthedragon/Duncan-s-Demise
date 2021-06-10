using System.Collections;
using UnityEngine;

public class ShieldAttack : Ability
{
    public override IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        base.ExecuteAbility(orbit, dir, damage);
        Transform target = orbit.transform.GetChild(0);
        Vector2 defaultPosition = target.localPosition;
        Quaternion defaultRotation = target.rotation;

        target.localPosition = Vector2.zero;
        target.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(Duration);

        target.localPosition = defaultPosition;
        target.rotation = defaultRotation;

        orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
    }

    public override string GetDescription()
    {
        return "Block attacks";
    }
}
