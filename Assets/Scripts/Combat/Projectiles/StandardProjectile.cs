using UnityEngine;

public class StandardProjectile : Projectile
{
    private float duration;
    private float startSpeed;
    private bool isShot;

    private int damage;

    private float timer = 0;

    public void Update()
    {
        if (!isShot) return;

        transform.Translate(direction * startSpeed * Time.deltaTime);

        CountDown();
    }

    /// <summary>
    /// Prepares the projectile to shoot
    /// </summary>
    /// <param name="duration"> The duration of the projectile</param>
    /// <param name="speed"> The speed of the projectile</param>
    /// <param name="tag"> The object to potentially damage</param>
    public override void Prepare(float duration, float speed, string tag, int damage)
    {
        this.duration = duration;
        startSpeed = speed;
        attackTag = tag;
        this.damage = damage;

        isShot = true;
    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Attacks the player when the collision happens
    /// </summary>
    /// <param name="collision"> The collision</param>
    public override void Attack(Collider2D collision)
    {
        if (collision.CompareTag(attackTag))
        {
            Entity entity = collision.GetComponent<Entity>();

            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<Player>().ReceiveDamage(-damage, enemy);
            }
            else
            {
                if (!entity.HasInvincFrames)
                {
                    AudioPlayer.Audioplayer.PlaySFX(onHit, volume);
                }
                entity.ChangeHealth(-damage);
            }

            GameObject.Destroy(this.gameObject);
        }
    }
}
