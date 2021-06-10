using UnityEngine;

public class IdleState : EnemyState
{
    private Enemy enemy;

    private float timer = 0;

    public IdleState(Enemy enemy) : base(enemy)
    {
        this.enemy = enemy;
        enemy.HealthChanged += IncreaseRange;
        enemy.Rb2d.velocity = Vector2.zero;

        enemy.RegisterMovement(Vector2.zero);
        enemy.AttackAnimation(false);
    }

    public override void ExecuteState()
    {
        CheckForPlayer();
        enemy.Regen();
        CountDown();
    }

    /// <summary>
    /// Checks if the player is in range to follow the player
    /// </summary>
    private void CheckForPlayer()
    {
        Collider2D playerCollider = EnemyUtilities.CheckPlayerInRange(enemy);
        if (playerCollider == null) return;

        enemy.HealthChanged -= IncreaseRange;
        enemy.SetState(new FollowState(enemy, playerCollider.gameObject.GetComponent<Player>()));
    }

    /// <summary>
    /// Increases the range of detecting the player
    /// </summary>
    /// <param name="value"> Not used</param>
    private void IncreaseRange(int value)
    {
        if (value == enemy.MaxHP) return;

        if (enemy.FollowDistance != enemy.MaxDistanceFromSpawn)
        {
            enemy.PlayerDetectionRange = enemy.FollowDistance;
            enemy.FollowDistance = enemy.MaxDistanceFromSpawn;
        }
    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.IdleTime)
        {
            enemy.SetState(new WanderState(enemy));
        }
    }
}