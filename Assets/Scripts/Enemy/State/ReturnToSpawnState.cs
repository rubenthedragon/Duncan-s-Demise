using UnityEngine;

public class ReturnToSpawnState : EnemyState
{
    private Enemy enemy;

    public ReturnToSpawnState(Enemy enemy) : base(enemy)
    {
        this.enemy = enemy;
        enemy.AttackAnimation(false);
    }
    public override void ExecuteState()
    {
        CheckForPlayer();
        enemy.Regen();
    }

    public override void ExecuteFixedState()
    {
        if (enemy.IsExperiencingKnockback) return;

        if (Vector2.Distance(enemy.transform.position, enemy.SpawnLocation) > 0.1f)
        {
            enemy.Rb2d.MovePosition(
                       (Vector2)enemy.transform.position +
                       (enemy.SpawnLocation - (Vector2)enemy.transform.position).normalized
                       * enemy.MovementSpeed * Time.fixedDeltaTime);

            enemy.RegisterMovement((enemy.SpawnLocation - (Vector2)enemy.transform.position).normalized);
        }

    }

    /// <summary>
    /// Checks if the player is in range when walking back to follow the player
    /// </summary>
    private void CheckForPlayer()
    {
        Collider2D playerCollider = EnemyUtilities.CheckPlayerInRange(enemy);

        if (playerCollider != null)
        {
            enemy.SetState(new FollowState(enemy, playerCollider.gameObject.GetComponent<Player>()));
        }
        else
        {
            if (Vector2.Distance(enemy.transform.position, enemy.SpawnLocation) <= 0.1f)
            {
                enemy.SetState(new IdleState(enemy));
            }
        }
    }
}