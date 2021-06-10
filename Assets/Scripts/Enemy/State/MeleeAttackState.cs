using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeAttackState : EnemyState
{
    private Enemy enemy;
    private Player player;

    public MeleeAttackState(Enemy enemy, Player player) : base(enemy)
    {
        this.enemy = enemy;
        this.player = player;
        enemy.Rb2d.velocity = Vector2.zero;
        enemy.RegisterMovement(Vector2.zero);

        enemy.AttackAnimation(true);
        Attack();
    }

    public override void ExecuteState()
    {
        enemy.LookAt(player.transform.position);

        if (!enemy.IsExperiencingKnockback)
        {
            enemy.Rb2d.velocity = Vector2.zero;
        }

        if (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.MeleeAttackRange)
        {
            Attack();

            if (enemy.RetreatTime > 0)
            {
                enemy.SetState(new RetreatState(enemy, player));
            }
            else if (enemy.MeleeAttacks.Length == 0 && !enemy.StandardMeleeAttack)
            {
                if (Vector2.Distance(enemy.transform.position, player.transform.position) <= enemy.MaxDistanceFromSpawn)
                {
                    enemy.SetState(new RangedAttackState(enemy, player));
                }
            }
        }
        else if (Vector2.Distance(enemy.transform.position, player.transform.position) > enemy.MeleeAttackRange * 1.2f)
        {
            enemy.AttackAnimation(false);
            enemy.SetState(new FollowState(enemy, player));
        }
    }

    /// <summary>
    /// Attack where the player is
    /// </summary>
    private void Attack()
    {
        if (!enemy.CanMeleeAttack || enemy.MeleeAttacks.Length == 0) return;

        MeleeAttack randomMelee = enemy.MeleeAttacks[Random.Range(0, enemy.MeleeAttacks.Length)];
        enemy.CurrentMeleeAttack = randomMelee;
        enemy.CanMeleeAttack = false;
        enemy.CurrentMeleeAttack.Damage = (int)((enemy.Damage * enemy.CurrentMeleeAttack.DamageModifier) * Random.Range(0.9f, 1.1f));
        enemy.CurrentMeleeAttack.enemy = enemy;
        AttackPoint attackPoint = GameObject.Instantiate(randomMelee.AttackPoint);
        attackPoint.Activate(randomMelee, enemy, player.transform.position);
    }

}