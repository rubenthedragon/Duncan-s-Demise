using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class Ability : MonoBehaviour
{
    [field: SerializeField] public AbilityType abilityType { get; set; }
    [field: SerializeField] public float MinDamageModifier { get; set; }
    [field: SerializeField] public float MaxDamageModifier { get; set; }
    [field: SerializeField] public float KnockbackThrust { get; set; }
    [field: SerializeField] public float KnockbackTime { get; set; }
    [field: SerializeField] public float Duration { get; set; }
    [field: SerializeField] public float CoolDown { get; set; }
    [field: SerializeField] public int ManaCost { get; set; }
    [field: SerializeField] public int msInvincibility { get; set; }
    [field: SerializeField] public bool canReflectProjectiles { get; set; }
    [SerializeField] private AudioClip onExeggcute;
    [SerializeField] private float volume;

    public virtual IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        if(onExeggcute != null)
        AudioPlayer.Audioplayer.PlaySFX(onExeggcute, volume);
        return null;
    }

    public abstract string GetDescription();
}