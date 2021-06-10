using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttackpoint : AttackPoint
{
    [SerializeField] private Animator animator;
    [SerializeField] private float waitTime;

    private Enemy enemy;

    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        if (enemy.GetComponentsInChildren<AttackPoint>().Length > 0)
        {
            GameObject.Destroy(this);
        }
        else
        {
            this.meleeAttack = meleeAttack;
            this.enemy = enemy;
            transform.SetParent(enemy.transform);
            transform.position = enemy.transform.position;
            active = true;
        }
    }

    public override void StartAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        attackNow = true;
        enemy.CanReceiveDamage = false;
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Deplete", true);
    }

    protected override void AttackTime()
    {
        if (!attackNow) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= meleeAttack.AttackTime)
        {
            animator.SetBool("Deplete", false);
            enemy.CanReceiveDamage = true;
            GameObject.Destroy(this.gameObject);
        }
    }

    public override void AttackPlayer(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null) return;

        player.ReceiveDamage(-meleeAttack.Damage, meleeAttack.enemy);
    }

}