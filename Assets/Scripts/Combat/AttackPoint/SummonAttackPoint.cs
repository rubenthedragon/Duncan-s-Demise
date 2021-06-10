using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAttackPoint : AttackPoint
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private float delay;

    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        this.meleeAttack = meleeAttack;
        transform.position = attackPosition;
        active = true;
    }

    public override void StartAttack() => StartCoroutine(Spawn());

    private IEnumerator Spawn()
    {
        attackNow = true;
        active = false;
        yield return new WaitForSeconds(delay);
        Spawner instantiatedSpawner = Instantiate(spawner, transform);
    }

}