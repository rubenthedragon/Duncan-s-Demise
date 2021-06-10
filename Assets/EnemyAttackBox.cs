using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        player.ReceiveDamage((int)-enemy.Damage, enemy);
    }
}