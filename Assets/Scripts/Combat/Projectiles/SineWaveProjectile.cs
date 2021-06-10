using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveProjectile : Projectile
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float amplitude;
    [SerializeField] private float waveLength;
    [SerializeField] private float waveSpeed;
    [SerializeField] private int positionCount = 200;

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

    public override void Prepare(float duration, float speed, string tag, int damage)
    {
        this.duration = duration;
        startSpeed = speed;
        attackTag = tag;
        this.damage = damage;

        Vector3 pos = transform.position;
        //Make the projectile visible above the sprites
        pos.z = -0.01f;
        transform.position = pos;
        
        isShot = true;
    }

    private void CountDown()
    {
        timer += Time.deltaTime;

        SineWave();

        if (timer >= duration)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void SineWave()
    {
        float x = 0f;
        float k = 2 * Mathf.PI / waveLength;
        float w = k * waveSpeed;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = positionCount;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            x += i * 0.001f;
            lineRenderer.SetPosition(
                i,
                new Vector3(x, amplitude * Mathf.Sin(k * x + w * Time.time))
                );
        }
    }

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
        else if (collision.gameObject.name.Equals("EdgeTilemap"))
        {
            GameObject.Destroy(this.gameObject);
        }

    }
}