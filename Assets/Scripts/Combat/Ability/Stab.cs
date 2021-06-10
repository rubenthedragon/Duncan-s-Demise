using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : Ability
{
    [SerializeField] private float stabDistance;
    [SerializeField, Min(1)] private int stabTimes;
    [SerializeField] private float speed;

    public override IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        base.ExecuteAbility(orbit, dir, damage);
        Transform itemTransform = orbit.CurrentCombatItemObject.transform;
        Vector2 startposition = itemTransform.localPosition;
        Vector2 stabPosition = new Vector2(itemTransform.localPosition.x, itemTransform.localPosition.y + stabDistance);


        for (int i = 0; i < stabTimes; i++)
        {
            while (Vector2.Distance(itemTransform.localPosition, stabPosition) > 0.1f)
            {
                itemTransform.localPosition = Vector2.MoveTowards(itemTransform.localPosition, stabPosition, (speed / Duration * stabTimes) * Time.deltaTime);
                yield return null;
            }

            while (Vector2.Distance(itemTransform.localPosition, startposition) > 0.1f)
            {
                itemTransform.localPosition = Vector2.MoveTowards(itemTransform.localPosition, startposition, (speed / Duration * stabTimes) * Time.deltaTime);
                yield return null;
            }

            itemTransform.localPosition = startposition;
        }

        orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
    }

    public override string GetDescription()
    {
        return string.Format("Stab {0} {1}  {2}MP", stabTimes, "time" + (stabTimes > 1 ? "s" : ""), ManaCost);
    }
}