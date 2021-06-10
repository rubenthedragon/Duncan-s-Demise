using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttackPoint : AttackPoint
{
    [SerializeField] private Animator animator;
    [SerializeField] private float appearLength;
    [SerializeField] private float waitTime;

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
     
    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        this.meleeAttack = meleeAttack;
        transform.position = attackPosition;

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        active = true;
    }

    public override void StartAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(appearLength);
        attackNow = true;
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Done", true);
    }
}
