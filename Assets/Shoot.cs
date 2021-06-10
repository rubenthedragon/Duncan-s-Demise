using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Ability
{
    [SerializeField] private RangedAttack rangedAttack;
    [SerializeField] private string shootName;
    [SerializeField] private float rotationOffset;
    [SerializeField] private bool stopItemRotation;

    /// <summary>
    /// Throws a projectile in the direction of the mouse
    /// </summary>
    /// <param name="orbit"> The player orbit</param>
    /// <param name="dir"> Not used</param>
    /// <returns></returns>
    public override IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        base.ExecuteAbility(orbit, dir, damage);
        if (!stopItemRotation)
        {
            orbit.CurrentCombatItemObject.gameObject.transform.Rotate(0, 0, rotationOffset);
        }

        rangedAttack.Shoot(orbit.transform.position, orbit.DefaultArrow.transform.position, "Enemy", damage);

        float time = 0.0f;

        while (time < Duration)
        {
            time += Time.deltaTime;

            if (stopItemRotation)
            {
                orbit.CurrentCombatItemObject.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, rotationOffset + orbit.transform.rotation.z);
            }

            yield return null;
        }

        orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
    }

    public override string GetDescription()
    {
        return string.Format("Shoot {0} {1}  {2}MP", rangedAttack.GetAmount(), shootName + (rangedAttack.GetAmount() > 1 ? "s" : ""), ManaCost);
    }
}