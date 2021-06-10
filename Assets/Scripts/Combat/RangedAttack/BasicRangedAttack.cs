using UnityEngine;

public class BasicRangedAttack : RangedAttack
{
    [SerializeField, Min(1)] private int amount;
    [SerializeField] private float range;
    [SerializeField] private float angleOffset;

    /// <summary>
    /// Shoots the projectile to a direction
    /// </summary>
    /// <param name="startPoint"> The point to start</param>
    /// <param name="destination"> The destination to shoot at</param>
    public override void Shoot(Vector2 startPoint, Vector2 destination, string tag, int damage)
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = Mathf.Atan2(destination.y - startPoint.y, destination.x - startPoint.x) * Mathf.Rad2Deg;
            if (amount > 1) angle = angle - angleOffset - (range / 2) + (i * (range / (amount - 1)));

            Projectile initProjectile = GameObject.Instantiate(projectile, startPoint, Quaternion.Euler(new Vector3(0, 0, angle)));

            initProjectile.Prepare(projectileDuration, projectileSpeed, tag, damage);
        }
    }

    public override void Shoot(Vector2 startPoint, Vector2 destination, string tag, int damage, Enemy enemy)
    {
        startPoint.y -= yOffset;

        for (int i = 0; i < amount; i++)
        {
            float angle = Mathf.Atan2(destination.y - startPoint.y, destination.x - startPoint.x) * Mathf.Rad2Deg;
            if (amount > 1) angle = angle - angleOffset - (range / 2) + (i * (range / (amount - 1)));

            Projectile initProjectile = GameObject.Instantiate(projectile, startPoint, Quaternion.Euler(new Vector3(0, 0, angle)));
            initProjectile.enemy = enemy;

            initProjectile.Prepare(projectileDuration, projectileSpeed, tag, damage);
        }

    }

    public override int GetAmount()
    {
        return amount;
    }
}