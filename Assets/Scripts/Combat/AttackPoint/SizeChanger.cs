using UnityEngine;

public class SizeChanger : AttackPoint
{
    [SerializeField] private float sizeIncrease;

    private float prepareTimer = 0;

    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        this.meleeAttack = meleeAttack;
        transform.position = attackPosition;
        active = true;
    }

    public override void StartAttack()
    {
        if (attackNow) return;

        prepareTimer += Time.deltaTime;

        if (prepareTimer < meleeAttack.TimeBeforeAttack)
        {
            float size = Mathf.Lerp(1, sizeIncrease, prepareTimer / meleeAttack.TimeBeforeAttack);
            transform.localScale = new Vector3(size, size, 1);
        }
        else
        {
            attackNow = true;
        }
    }
}
