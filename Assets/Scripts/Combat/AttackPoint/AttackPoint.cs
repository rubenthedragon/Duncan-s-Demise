using UnityEngine;

public abstract class AttackPoint : MonoBehaviour
{
    [SerializeField] private Vector3 size = Vector3.one;

    protected MeleeAttack meleeAttack;
    protected bool active;
    protected bool attackNow;

    protected float attackTimer = 0;

    private void Awake() => transform.localScale = size;

    private void Update()
    {
        if (!active) return;
        StartAttack();
        AttackTime();
    }

    public abstract void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackNow || !collision.isTrigger) return;
        AttackPlayer(collision); 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackNow || !collision.isTrigger) return;
        AttackPlayer(collision); 
    }

    /// <summary>
    /// Start the attack animation
    /// </summary>
    public abstract void StartAttack();

    /// <summary>
    /// The time the attack should last
    /// </summary>
    protected virtual void AttackTime()
    {
        if (!attackNow) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= meleeAttack.AttackTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Attack the player on collision
    /// </summary>
    /// <param name="collision"> The collision</param>
    public virtual void AttackPlayer(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null) return;

        player.ReceiveDamage(-meleeAttack.Damage, meleeAttack.enemy);

        GameObject.Destroy(this.gameObject);
    }
}