using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AlphaChangingAttackPoint : AttackPoint
{
    private SpriteRenderer spriteRenderer;
    private void Start() => spriteRenderer = GetComponent<SpriteRenderer>();

    private float prepareTimer = 0;

    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        this.meleeAttack = meleeAttack;
        transform.position = attackPosition;
        active = true;
    }

    /// <summary>
    /// Start the attack animation
    /// </summary>
    public override void StartAttack()
    {
        if (attackNow) return;

        prepareTimer += Time.deltaTime;

        if (prepareTimer < meleeAttack.TimeBeforeAttack)
        {
            Color color = spriteRenderer.color;
            color.a = prepareTimer / meleeAttack.TimeBeforeAttack;
            spriteRenderer.color = color;
        }
        else
        {
            attackNow = true;
        }
    }
}
