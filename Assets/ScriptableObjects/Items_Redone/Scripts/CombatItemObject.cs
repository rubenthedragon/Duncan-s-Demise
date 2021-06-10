using UnityEngine;

public class CombatItemObject : MonoBehaviour
{
    [field: SerializeField] public Ability LeftAbility { get; private set; }
    [field: SerializeField] public Ability RightAbility { get; private set; }
    [SerializeField] private AudioClip HitSound;
    [SerializeField] private float HitSoundVolume;

    [HideInInspector] public int MinDamage { get; set; }
    [HideInInspector] public int MaxDamage { get; set; }

    public Player player { get; set; }

    private Ability lastUsedAbility;

    private int damage;

    /// <summary>
    /// Lets the player use the abilities on the current combatItem
    /// </summary>
    /// <param name="orbit"> The orbit of the player</param>
    /// <param name="dir"> Which ability should be used</param>
    public void UseAbility(Orbit orbit, Direction dir)
    {
        lastUsedAbility = dir == Direction.Right ? RightAbility : LeftAbility;

        int minDamage = Mathf.FloorToInt(MinDamage + lastUsedAbility.MinDamageModifier * GetStat());
        int maxDamage = Mathf.CeilToInt(MaxDamage + lastUsedAbility.MaxDamageModifier * GetStat());

        damage = Random.Range(minDamage, maxDamage + 1);

        StartCoroutine(player.GiveInvincFrames(lastUsedAbility.msInvincibility));
        StartCoroutine(lastUsedAbility.ExecuteAbility(orbit, dir, damage));
    }

    public bool EnoughMana(Ability ability)
    {
        if (player.Mana >= ability.ManaCost)
        {
            player.ChangeMana(-ability.ManaCost);
            return true;
        }
        return false;
    }

    public Ability GetUsedAbility(Direction dir)
    {
        return dir == Direction.Right ? RightAbility : LeftAbility;
    }


    /// <summary>
    /// Applies the combatItem effects when an enemy collides
    /// </summary>
    /// <param name="other"> The collided object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (lastUsedAbility == null) return;
        if (other.gameObject.GetComponent<Enemy>() != null && other.isTrigger)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (!enemy.HasInvincFrames)
            {
                if (HitSoundVolume == 0f)
                {
                    HitSoundVolume = 0.05f;
                }
                AudioPlayer.Audioplayer.PlaySFX(HitSound, HitSoundVolume);
            }
            enemy.ChangeHealth(-damage);
            enemy.KnockBack(transform.position, lastUsedAbility.KnockbackThrust, lastUsedAbility.KnockbackTime);
        }

        if (lastUsedAbility.canReflectProjectiles && other.gameObject.GetComponent<Projectile>() != null)
        {
            other.gameObject.GetComponent<Projectile>().Reflect();
        }
    }

    public int GetStat()
    {
        switch (lastUsedAbility.abilityType)
        {
            case AbilityType.Melee:
                return player.Strength;
            case AbilityType.Ranged:
                return player.Dexterity;
            case AbilityType.Magic:
                return player.Intelligence;
            default:
                return 0;
        }
    }
}