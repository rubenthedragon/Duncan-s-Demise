using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField] public float TimeBeforeAttack { get; private set; }
    [field: SerializeField] public float AttackTime { get; private set; }
    [field: SerializeField] public float DamageModifier { get; private set; }
    [HideInInspector] public int Damage { get; set; }
    [field: SerializeField] public AttackPoint AttackPoint { get; private set; }
    [HideInInspector] public Enemy enemy { get; set; }
}
