using Unity.Mathematics;
using UnityEngine;

public class RotationAttack : AttackPoint
{
    [SerializeField] private float targetRotation;

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
            float rotation = Mathf.Lerp(0, targetRotation, prepareTimer / meleeAttack.TimeBeforeAttack);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);
        }
        else
        {
            attackNow = true;
        }
    }
}
