using UnityEngine;

public class RangedAttackState : EnemyState
{
    private Enemy enemy;
    private Player player;

    private float timer = 0;
    private float time;

    public RangedAttackState(Enemy enemy, Player player) : base(enemy)
    {
        this.enemy = enemy;
        this.player = player;
        enemy.Rb2d.velocity = Vector2.zero;

        enemy.RegisterMovement(Vector2.zero);

        if (enemy.RangedAttacks.Length == 0)
        {
            enemy.AttackAnimation(false);
            enemy.SetState(new FollowState(enemy, player));
        }
        else
        {
            Attack();
        }

    }

    public override void ExecuteState()
    {
        enemy.LookAt(player.transform.position);

        if (!enemy.IsExperiencingKnockback)
        {
            enemy.Rb2d.velocity = Vector2.zero;
        }

        CountDown();

        if (Vector2.Distance(enemy.transform.position, player.transform.position) <= enemy.MeleeAttackRange)
        {
            enemy.SetState(new MeleeAttackState(enemy, player));
        }
    }

    /// <summary>
    /// Do the ranged attack
    /// </summary>
    private void Attack()
    {
        if (!enemy.CanRangeAttack || enemy.RangedAttacks.Length == 0) return;
        enemy.AttackAnimation(true);
        enemy.CanRangeAttack = false;

        RangedAttack randomRanged = enemy.RangedAttacks[Random.Range(0, enemy.RangedAttacks.Length)];
        enemy.CurrentRangedAttack = randomRanged;
        time = enemy.CurrentRangedAttack.EnemyCooldown;
        int damage = (int)((enemy.Damage * enemy.CurrentRangedAttack.damageModifier) * Random.Range(0.9f, 1.1f));
        enemy.CurrentRangedAttack.Shoot(enemy.transform.position, player.transform.position, "Player", damage, enemy);

    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= time)
        {
            enemy.AttackAnimation(false);
            enemy.SetState(new FollowState(enemy, player));
        }
    }
}