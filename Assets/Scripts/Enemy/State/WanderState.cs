using UnityEngine;

public class WanderState : EnemyState
{
    private Enemy enemy;
    private Vector2 wanderPoint;

    public WanderState(Enemy enemy) : base(enemy)
    {
        this.enemy = enemy;
        enemy.HealthChanged += IncreaseRange;
        this.wanderPoint = NewWanderPoint();

        enemy.OnCollisionEnter += OnCollisionEnter2D;

        enemy.Rb2d.velocity = Vector2.zero;
        enemy.AttackAnimation(false);
    }

    public override void ExecuteState()
    {
        CheckForPlayer();
        enemy.Regen();
        WanderToPoint();
    }

    /// <summary>
    /// Checks if the player is in range to follow the player
    /// </summary>
    private void CheckForPlayer()
    {
        Collider2D playerCollider = EnemyUtilities.CheckPlayerInRange(enemy);
        if (playerCollider == null) return;

        enemy.HealthChanged -= IncreaseRange;
        enemy.OnCollisionEnter -= OnCollisionEnter2D;
        enemy.SetState(new FollowState(enemy, playerCollider.gameObject.GetComponent<Player>()));
    }

    /// <summary>
    /// Increases the range of detecting the player
    /// </summary>
    /// <param name="value"> Check if the enemy was hit</param>
    private void IncreaseRange(int value)
    {
        if (value == enemy.MaxHP) return;


        if (enemy.FollowDistance != enemy.MaxDistanceFromSpawn)
        {
            enemy.PlayerDetectionRange = enemy.FollowDistance;
            enemy.FollowDistance = enemy.MaxDistanceFromSpawn;
        }
    }

    /// <summary>
    /// Lets the enemy wander to a point
    /// </summary>
    private void WanderToPoint()
    {
        if (Vector2.Distance(enemy.transform.position, wanderPoint) < 0.1f)
        {
            if (Random.value > 0.5f)
            {
                enemy.OnCollisionEnter -= OnCollisionEnter2D;
                enemy.SetState(new IdleState(enemy));
            }
            else
            {
                wanderPoint = NewWanderPoint();
            }
        }
        else
        {
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                wanderPoint,
                enemy.MovementSpeed / 2 * Time.deltaTime);

            enemy.RegisterMovement((wanderPoint - (Vector2)enemy.transform.position).normalized);
        }
    }

    /// <summary>
    /// Sets a new wander point for the enemy to wander to
    /// </summary>
    /// <returns> A new wander point</returns>
    private Vector2 NewWanderPoint()
    {
        return new Vector2(
            Random.Range(enemy.SpawnLocation.x - enemy.WanderRange, enemy.SpawnLocation.x + enemy.WanderRange),
            Random.Range(enemy.SpawnLocation.y - enemy.WanderRange, enemy.SpawnLocation.y + enemy.WanderRange));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemy.OnCollisionEnter -= OnCollisionEnter2D;
        enemy.SetState(new ReturnToSpawnState(enemy));
    }
}