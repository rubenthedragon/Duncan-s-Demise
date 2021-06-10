using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector2 direction = Vector2.right;
    protected string attackTag;
    [HideInInspector] public Enemy enemy;
    [SerializeField] protected AudioClip onHit;
    [SerializeField] protected float volume;

    public abstract void Prepare(float duration, float speed, string tag, int damage);
    public abstract void Attack(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") || collision.CompareTag("EnemyAttackBox")) return;
        if (
            collision.gameObject.name.Equals("WaterTilemap") || 
            collision.gameObject.GetComponent<AudioArea>() ||
            collision.gameObject.GetComponent<Spawner>() ||
            collision.gameObject.GetComponent<OnEnterSave>() ||
            collision.gameObject.GetComponent<OnEnterNextScene>()
            ) return;    
        Attack(collision);

        if (!collision.gameObject.GetComponent<Entity>() && !collision.gameObject.GetComponent<CombatItemObject>() && !collision.gameObject.GetComponent<GroundItem>())
        {
            Destroy(this.gameObject);
        }
    }
    public void Reflect()
    {
        direction = Vector2.left;
        attackTag = "Enemy";
    }
}