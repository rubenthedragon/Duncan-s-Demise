using UnityEngine;

public class ChargeState : EnemyState
{
    private Enemy enemy;
    private Player player;

    public ChargeState(Enemy enemy, Player player) : base(enemy)
    {
        this.enemy = enemy;
        this.player = player;

        if (!enemy.IsExperiencingKnockback)
        {
            enemy.Rb2d.velocity = Vector2.zero;
        }

        enemy.AttackAnimation(false);
    }

    public override void ExecuteState()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.SpawnLocation) < 0.1f)
        {
            enemy.SetState(new FollowState(enemy, player));
        }
    }

    public override void ExecuteFixedState()
    {
        if (enemy.IsExperiencingKnockback) return;

        enemy.Rb2d.MovePosition(
            (Vector2) enemy.transform.position +
            (enemy.SpawnLocation - (Vector2) enemy.transform.position).normalized
            * enemy.MovementSpeed * enemy.ChargeSpeedMultiplier * Time.fixedDeltaTime
            );

        enemy.RegisterMovement((enemy.SpawnLocation - (Vector2)enemy.transform.position).normalized);
    }

}