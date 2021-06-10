using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorChangingAttackPoint : AttackPoint
{
    [SerializeField] private Color32 colorToChangeTo;

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
            spriteRenderer.color =
                Color32.Lerp(
                    spriteRenderer.color,
                colorToChangeTo,
                (prepareTimer / meleeAttack.TimeBeforeAttack) / 255
                );
        }
        else
        {
            attackNow = true;
        }
    }
}
