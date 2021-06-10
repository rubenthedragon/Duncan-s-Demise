using System.Linq;
using UnityEngine;

public static class EnemyUtilities
{
    /// <summary>
    /// Checks if the player is in range
    /// </summary>
    /// <param name="enemy"> The enemy to check from</param>
    /// <returns> The player collider</returns>
    public static Collider2D CheckPlayerInRange(Enemy enemy)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.PlayerDetectionRange);

        Collider2D playerCollider =
            Physics2D.OverlapCircleAll(enemy.transform.position, enemy.PlayerDetectionRange)
            .Where(c => c.GetComponent<Player>() != null)
            .Where(c => c.isTrigger)
            .SingleOrDefault();

        if (playerCollider != null)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(enemy.transform.position, (playerCollider.transform.position - enemy.transform.position).normalized, Vector2.Distance(enemy.transform.position, playerCollider.transform.position));

            if (hits.Where(
                h => !h.collider.gameObject.CompareTag("Enemy"))
                .Where(h=> h.collider.gameObject.CompareTag("EnemyCollision"))
                .Count() == 0)
            {
                return playerCollider;
            }
        }

        return null;
    }

    /// <summary>
    /// Moves the enemy to its target
    /// </summary>
    /// <param name="enemy"> The enemy to move</param>
    /// <param name="target"> The target to move to</param>
    public static void MoveToTarget(Enemy enemy, Vector2 target)
    {
        enemy.transform.position =
            Vector2.MoveTowards(
                enemy.transform.position,
                target,
                enemy.MovementSpeed * Time.deltaTime
                );
    }
}