using UnityEngine;

public class FollowState : EnemyState
{
    private Enemy enemy;
    private Player player;

    private float timer = 0;

    public FollowState(Enemy enemy, Player player) : base(enemy)
    {
        this.enemy = enemy;
        this.player = player;

        enemy.AttackAnimation(false);
    }

    public override void ExecuteState()
    {
        FollowPlayer();
        CountDown();
    }

    public override void ExecuteFixedState()
    {
        if (enemy.IsExperiencingKnockback || enemy.FollowTime <= 0) return;

        enemy.Rb2d.MovePosition(
            enemy.transform.position +
            (player.transform.position - enemy.transform.position).normalized
            * enemy.MovementSpeed * Time.fixedDeltaTime);

        enemy.RegisterMovement((player.transform.position - enemy.transform.position).normalized);
    }

    /// <summary>
    /// Follows the player if the player is in range
    /// </summary>
    private void FollowPlayer()
    {
        if (enemy.FollowTime <= 0) return;

        Vector2 playerPosition = player.transform.position;

        if (Vector2.Distance(enemy.transform.position, playerPosition) >= enemy.FollowDistance)
        {
            enemy.SetState(new ReturnToSpawnState(enemy));
        }

        if (Vector2.Distance(enemy.SpawnLocation, enemy.transform.position) >= enemy.MaxDistanceFromSpawn)
        {
            if (Vector2.Distance(player.transform.position, enemy.SpawnLocation) <= enemy.MaxDistanceFromSpawn)
            {
                enemy.SetState(new ChargeState(enemy, player));
            }
            else
            {
                enemy.transform.position = enemy.SpawnLocation;
                enemy.SetState(new IdleState(enemy));
            }
        }

        if (Vector2.Distance(enemy.transform.position, playerPosition) < enemy.MeleeAttackRange)
        {
            enemy.Rb2d.velocity = Vector2.zero;
            enemy.SetState(new MeleeAttackState(enemy, player));
        }

    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.FollowTime)
        {
            if (Vector2.Distance(enemy.transform.position, player.transform.position) <= enemy.MaxDistanceFromSpawn)
            {
                enemy.SetState(new RangedAttackState(enemy, player));
            }
            else {
                enemy.SetState(new ReturnToSpawnState(enemy));
            }
        }
    }
}