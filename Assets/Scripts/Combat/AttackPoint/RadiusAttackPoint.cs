using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadiusAttackPoint : AttackPoint
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int segments = 360;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float minWidth;
    [SerializeField] private float maxWidth;
    [SerializeField] private float resizeSpeed;
    [SerializeField] private float widthResizeSpeed;

    [SerializeField] private CircleCollider2D circleCollider;

    private Enemy enemy;

    private float prepareTimer;

    private Vector3[] points;
    private float currentRadius;

    public override void Activate(MeleeAttack meleeAttack, Enemy enemy, Vector2 attackPosition)
    {
        this.meleeAttack = meleeAttack;
        this.enemy = enemy;

        transform.Rotate(new Vector3(90, 0, 0));
        transform.position = new Vector3(attackPosition.x, attackPosition.y, -0.0001f);
        active = true;

        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1;

        points = new Vector3[segments + 1];

        lineRenderer.SetPositions(RecalculatePoints(minRadius));
    }

    public override void StartAttack()
    {
        if (attackNow) return;

        prepareTimer += Time.deltaTime;

        if (prepareTimer >= meleeAttack.TimeBeforeAttack)
        {
            attackNow = true;
        }
    }


    protected override void AttackTime()
    {
        if (!attackNow) return;

        attackTimer += Time.deltaTime;

        lineRenderer.SetPositions(RecalculatePoints(Mathf.Lerp(minRadius, maxRadius, attackTimer * resizeSpeed)));

        float width = Mathf.Lerp(minWidth, maxWidth, attackTimer * widthResizeSpeed);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        if (attackTimer >= meleeAttack.AttackTime)
        {
            enemy.CanReceiveDamage = true;
            GameObject.Destroy(this.gameObject);
        }
    }

    private Vector3[] RecalculatePoints(float radius)
    {
        for (int i = 0; i < segments + 1; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        currentRadius = radius;
        circleCollider.radius = radius;

        return points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("EdgeTilemap"))
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public override void AttackPlayer(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null) return;

        player.ReceiveDamage(-meleeAttack.Damage, meleeAttack.enemy);
        GameObject.Destroy(this.gameObject);
    }
}