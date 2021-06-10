
using System.Collections;
using UnityEngine;

public class Parry : Ability
{
    public override IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        base.ExecuteAbility(orbit, dir, damage);
        Transform target = orbit.transform.GetChild(0);
        Vector2 defaultPosition = target.localPosition;
        Quaternion defaultRotation = target.rotation;

        target.localPosition = Vector2.zero;
        target.rotation = Quaternion.Euler(0, 0, 0);

        while (Vector2.Distance(target.localPosition, defaultPosition) > 0.1f)
        {
            target.localPosition = Vector2.MoveTowards(target.localPosition, defaultPosition, Duration * 2 * Time.deltaTime);
            yield return null;
        }

        target.localPosition = defaultPosition;
        target.rotation = defaultRotation;

        orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
    }

    public override string GetDescription()
    {
        return string.Format("Reflect projectiles");
    }
}
