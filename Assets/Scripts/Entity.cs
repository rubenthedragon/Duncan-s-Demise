using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IOnLoadAndSave
{
    public event Action<int> HealthChanged;
    public event Action<int> ManaChanged;

    private int health;
    private int mana;
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxMana;
    [SerializeField] private int msInvincOnDamage = 0; 

    public int MaxHP { get { return maxHealth; } set { maxHealth = value; } }
    public int HP { get { return health; } set { health = value; } }
    public int MaxMana { get { return maxMana; } set { maxMana = value; } }
    public int Mana { get { return mana;} set { mana = value; } }
    [field: SerializeField] public int Level { get; protected set; } = 1;
    [field: SerializeField] public float MovementSpeed { get; set; }
    [field: SerializeField] public Rigidbody2D Rb2d { get; protected set; }
    public bool HasInvincFrames {get; private set;}
    public bool CanReceiveDamage { get; set; } = true;


    public bool IsExperiencingKnockback { get; private set; } = false;

    private void Awake()
    {
        HP = MaxHP;
        Mana = MaxMana;
    }

    /// <summary>
    /// Apply knockback to the entity
    /// </summary>
    /// <param name="position"> The position of the other object</param>
    /// <param name="thrust"> The thrust of the knockback</param>
    /// <param name="time"> The time the knockback should last</param>
    public void KnockBack(Vector2 position, float thrust, float time)
    {
        IsExperiencingKnockback = true;
        Vector2 direction = (Vector2)transform.position - position;
        direction = direction.normalized * thrust;
        Rb2d.AddForce(direction, ForceMode2D.Impulse);

        StartCoroutine(KnockbackTime(time));
    }

    /// <summary>
    /// Applies how long the knockback lasts
    /// </summary>
    /// <param name="time"> Knockback time</param>
    /// <returns></returns>
    private IEnumerator KnockbackTime(float time)
    {
        yield return new WaitForSeconds(time);
        IsExperiencingKnockback = false;
    }


    /// <summary>
    /// Changes the health of the entity
    /// </summary>
    /// <param name="value"> The damage or healing value</param>
    public void ChangeHealth(int value)
    {
        if(value == 0) return;
        if (value < 0 && !CanReceiveDamage) return;

        if (value < 0)
        {
            if (HasInvincFrames && msInvincOnDamage > 0) return;
            StartCoroutine(GiveInvincFrames(msInvincOnDamage));
        }
        if (HP + value < 0)
        {
            HP = 0;
        }
        else if (HP + value >= MaxHP)
        {
            HP = MaxHP;
        }
        else
        {
            HP += value;
        }

        HealthChanged?.Invoke(value);
    }

    /// <summary>
    /// Changes the mana of the entity
    /// </summary>
    /// <param name="value"> The value to change with</param>
    public void ChangeMana(int value)
    {
        if (Mana + value < 0)
        {
            Mana = 0;
        }
        else if (Mana + value >= MaxMana)
        {
            Mana = MaxMana;
        }
        else
        {
            Mana += value;
        }

        ManaChanged?.Invoke(value);
    }

    public IEnumerator GiveInvincFrames(int milliseconds){
        HasInvincFrames = true;
        yield return new WaitForSeconds((float)milliseconds/1000);
        HasInvincFrames = false;
    }

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.GetComponent<Entity>() == null) return;
    //     Physics2D.IgnoreCollision(
    //         collider,
    //         GetComponents<Collider2D>()
    //         .Where(c => !c.isTrigger)
    //         .SingleOrDefault()
    //         );
    // }

    public abstract void Load();

    public abstract void Save();

    public abstract void OnEnable();

    public abstract void OnDisable();
}