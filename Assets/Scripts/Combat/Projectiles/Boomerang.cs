using UnityEngine;

public class Boomerang : Projectile
{
    private float duration;
    private float startSpeed;
    private string attackTag;
    private bool isShot;
    private Quaternion orgRotation;

    [SerializeField] private int rotationSpeed = 10; 

    private Vector3 curRotation = Vector3.forward;
    private Vector2 direction;

    private int damage;

    private float timer = 0;

    public void Start() => orgRotation = this.gameObject.transform.rotation;
    public void Update()
    {
        if (!isShot) return;

        transform.SetPositionAndRotation(this.transform.position, orgRotation);
        transform.Translate(direction * startSpeed * Time.deltaTime);
        transform.Rotate(curRotation += Vector3.forward * rotationSpeed, Space.Self);

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
        direction = Vector2.right;

        isShot = true;
    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        if (timer >= duration / 2) direction = Vector2.left;

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
                entity.ChangeHealth(-damage);

                if (!entity.HasInvincFrames)
                {
                    AudioPlayer.Audioplayer.PlaySFX(onHit, volume);
                }
            }

            GameObject.Destroy(this.gameObject);
        }
    }
}
