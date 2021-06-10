using UnityEngine;

public abstract class RangedAttack : MonoBehaviour
{
    [field: SerializeField] public float EnemyCooldown { get; private set; }
    [field: SerializeField] public Projectile projectile { get; protected set; }
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDuration;
    [SerializeField] protected float yOffset;
    public float damageModifier;

    /// <summary>
    /// Shoots the projectile to a direction
    /// </summary>
    /// <param name="startPoint"> The point to start</param>
    /// <param name="destination"> The destination to shoot at</param>
    public abstract void Shoot(Vector2 startPoint, Vector2 destination, string tag, int damage);

    public abstract void Shoot(Vector2 startPoint, Vector2 destination, string tag, int damage, Enemy enemy);


    public virtual int GetAmount() => 1;
}
