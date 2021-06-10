using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : EnemyState
{
    private Enemy enemy;
    private Player player;

    private float timer = 0;

    public RetreatState(Enemy enemy, Player player) : base(enemy)
    {
        this.enemy = enemy;
        this.player = player;

        enemy.AttackAnimation(false);
        enemy.OnCollisionEnter += StopRunningIntoWalls;
    }

    public override void ExecuteState()
    {
        CountDown();

        if (Vector2.Distance(enemy.transform.position, player.transform.position) >= enemy.FollowDistance)
        {
            enemy.SetState(new RangedAttackState(enemy, player));
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
    }

    public override void ExecuteFixedState()
    {
        RetreatFromPlayer();
    }

    private void StopRunningIntoWalls(Collision2D collider)
    {
        enemy.SetState(new RangedAttackState(enemy, player));
    }

    /// <summary>
    /// Retreats from the player to avoid being attacked with melee
    /// </summary>
    private void RetreatFromPlayer()
    {
        if (enemy.RetreatTime <= 0) return;

        Vector2 playerPosition = player.transform.position;

        Vector3 direction = playerPosition - (Vector2)enemy.transform.position;

        if (enemy.IsExperiencingKnockback) return;

        enemy.Rb2d.MovePosition(
            enemy.transform.position -
            direction.normalized * enemy.MovementSpeed
            * enemy.RetreatSpeedMultiplier * Time.fixedDeltaTime
            );

        enemy.RegisterMovement(((Vector2)enemy.transform.position - playerPosition).normalized);
    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.RetreatTime)
        {
            enemy.SetState(new MeleeAttackState(enemy, player));
        }
    }
}
