using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class Enemy : Entity
{
    private EnemyState currentState;

    public event Action<Collision2D> OnCollisionEnter;
    public event Action<Collision2D> OnCollisionStay;

    private float meleeTimer = 0;
    private float rangedTimer = 0;
    private float regenTimer = 0;

    [field: SerializeField, Header("Basic Settings")] public EnemyID EnemyID { get; set; }
    [SerializeField] private AudioClip source;
    [SerializeField] private float volume;
    [SerializeField] private Animator animator;

    [Header("Item Settings")]
    [SerializeField] private float itemDropRange;
    [SerializeField] private ListItem[] droptable;

    [field: SerializeField, Header("Damage Settings")] public float Damage { get; set; }
    [SerializeField] private int baseDamage;
    [SerializeField] private int walkThroughDamage;

    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackThrust;
    [SerializeField] protected float knockbackTime;

    [Header("Regen Settings")]
    [SerializeField] protected float timeBetweenRegen;
    [SerializeField] protected int regenIncrement;

    [field: SerializeField, Header("Attack Settings")] public RangedAttack[] RangedAttacks { get; private set; }
    [field: SerializeField] public bool StandardMeleeAttack { get; private set; }
    [field: SerializeField] public MeleeAttack[] MeleeAttacks { get; private set; }
    [field: SerializeField] public float MeleeAttackRange { get; private set; }
    [field: SerializeField] public float MaxDistanceFromSpawn { get; set; }
    [field: SerializeField] public float PlayerDetectionRange { get; set; }

    [field: SerializeField, Header("Movement Settings")] public float IdleTime { get; private set; }
    [field: SerializeField] public float FollowTime { get; private set; }
    [field: SerializeField] public float FollowDistance { get; set; }
    [field: SerializeField] public float WanderRange { get; private set; }

    [field: SerializeField, Header("Retreat Settings")] public float RetreatTime { get; private set; }
    [field: SerializeField] public float RetreatSpeedMultiplier { get; private set; }
    [field: SerializeField, Header("Charge Settings")] public float ChargeSpeedMultiplier { get; private set; }

    public Vector2 SpawnLocation { get; private set; }
    public bool CanMeleeAttack { get; set; } = true;
    public bool CanRangeAttack { get; set; } = true;

    public MeleeAttack CurrentMeleeAttack { get; set; }
    public RangedAttack CurrentRangedAttack { get; set; }

    private float oldRetreatTime;

    private void Start()
    {
        SpawnLocation = transform.position;
        oldRetreatTime = RetreatTime;
        SetState(new IdleState(this));
    }
    private void Update()
    {
        currentState.ExecuteState();
        Die();

        if (!CanMeleeAttack)
        {
            MeleeAttackCooldown();
        }

        if (!CanRangeAttack)
        {
            RangedAttackCooldown();
        }
    }

    private void FixedUpdate() => currentState.ExecuteFixedState();

    public void RegisterMovement(Vector2 movement)
    {
        if (animator == null) return;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
            SetDirection(Mathf.RoundToInt(movement.x), Mathf.RoundToInt(movement.y));
        }
    }

    public void LookAt(Vector2 lookAtPos)
    {
        if (animator == null) return;

        Vector2 direction = (lookAtPos - (Vector2)transform.position).normalized;
        animator.SetFloat("LastHorizontal", direction.x);
        animator.SetFloat("LastVertical", direction.y);

        SetDirection(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
    }


    private void SetDirection(float lastHorizontal, float lastVertical)
    {
        if (lastHorizontal == 0 && lastVertical == 1)
        {
            animator.SetInteger("Direction", 1);
        }

        if (lastHorizontal == -1 && lastVertical == 0)
        {
            animator.SetInteger("Direction", 2);
        }

        if (lastHorizontal == 1 && lastVertical == 0)
        {
            animator.SetInteger("Direction", 3);
        }

        if (lastHorizontal == 0 && lastVertical == -1)
        {
            animator.SetInteger("Direction", 4);
        }
    }

    public void AttackAnimation(bool active)
    {
        if (animator == null) return;

        animator.SetBool("Attacking", active);
    }

    /// <summary>
    /// Let the enemy die when the health drops to zero
    /// </summary>
    public void Die()
    {
        if (HP > 0) return;

        if (source != null)
        {
            AudioPlayer.Audioplayer.PlaySFX(source, volume);
        }

        Player player = FindObjectOfType<Player>();
        float m = player.Level == Level ?
            1 : player.Level > Level ?
            Mathf.Clamp(1 - (player.Level - Level) * 0.33f, 0, 1) : Mathf.Clamp(1 + (Level - player.Level) * 0.33f, 1, 2);

        if (Level != 0)
        {
            player.AddExp((int)((0.3 * Math.Pow(player.Level, 2) + 10 * player.Level + 10) * m * UnityEngine.Random.Range(0.9f, 1.1f)));
        }

        DropItems();
        GoalEventHandler.EnemyDied(this.EnemyID);

        Canvas HCDisplay = GetComponentInChildren<Canvas>();

        HPBar hpbar = HCDisplay.GetComponentInChildren<HPBar>();

        if (hpbar != null)
        {
            Destroy(hpbar.gameObject);
        }

        EnemyLevelUI levelUI = HCDisplay.GetComponentInChildren<EnemyLevelUI>();

        if (levelUI != null)
        {
            Destroy(levelUI.gameObject);
        }

        HCDisplay.transform.SetParent(transform.parent);
        HCDisplay.name = HCDisplay.name + $"(From: {name})";
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// Drops the enemy items in a range from the enemy
    /// </summary>
    private void DropItems()
    {
        foreach (ListItem item in droptable)
        {
            int tempRandomNr = UnityEngine.Random.Range(0, 100);
            int tempDropAmount = 1;
            if (tempRandomNr <= item.Dropchance)
            {
                if (item.MaxAmount == 1)
                {
                    tempDropAmount = 1;
                }
                else
                {
                    tempDropAmount = UnityEngine.Random.Range(0, item.MaxAmount);
                }

                for (int i = 0; i < tempDropAmount; i++)
                {
                    GroundItem groundItem = Instantiate(Resources.Load<GroundItem>("GroundItem"));
                    ItemObject instItem = Instantiate(item.Item);
                    instItem.name = item.Item.name;

                    groundItem.SetItem(instItem.type == ItemType.CombatItem ? AddItemRarity((CombatItem)instItem) : instItem);

                    float x = transform.position.x;
                    float y = transform.position.y;

                    Vector2 dropLocation = new Vector2(
                        UnityEngine.Random.Range(x - itemDropRange, x + itemDropRange),
                        UnityEngine.Random.Range(y - itemDropRange, y + itemDropRange)
                    );

                    groundItem.transform.position = dropLocation;
                }
            }
        }
    }

    /// <summary>
    /// Cooldown for meleeAttack
    /// </summary>
    private void MeleeAttackCooldown()
    {
        if (CurrentMeleeAttack == null) return;
        meleeTimer += Time.deltaTime;

        if (meleeTimer >= CurrentMeleeAttack.Cooldown)
        {
            CanMeleeAttack = true;
            meleeTimer = 0;
        }
    }
    /// <summary>
    /// Cooldown for rangedAttack
    /// </summary>
    private void RangedAttackCooldown()
    {
        if (CurrentRangedAttack == null) return;

        rangedTimer += Time.deltaTime;

        if (rangedTimer >= CurrentRangedAttack.EnemyCooldown)
        {
            CanRangeAttack = true;
            rangedTimer = 0;
        }
    }

    /// <summary>
    /// Sets the enemy state
    /// </summary>
    /// <param name="newState"> The new state to apply</param>
    public void SetState(EnemyState newState)
    {
        if (newState == null) return;

        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;

        currentState.PrepareState();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        RetreatTime = 0;

        Player player = col.gameObject.GetComponent<Player>();
        if (player == null || walkThroughDamage == 0) return;
        player.ReceiveDamage(-walkThroughDamage, this);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        RetreatTime = oldRetreatTime;
    }

    /// <summary>
    /// Draws debug wires to display the range of certain attributes
    /// NOTE: This is only visible in the editor when selecting the object
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, FollowDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, itemDropRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, MeleeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MaxDistanceFromSpawn);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, WanderRange);
    }

    /// <summary>
    /// Lets the enemy regenerate its health
    /// </summary>
    public void Regen()
    {
        if (HP >= MaxHP)
        {
            regenTimer = 0;
        }
        else
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= timeBetweenRegen)
            {
                ChangeHealth(regenIncrement);
                regenTimer = 0;
            }
        }
    }

    public void GiveLevelBetweenRange(int min, int max)
    {
        int newLevel = UnityEngine.Random.Range(min, max);
        Level = newLevel < 0 ? 0 : newLevel;
        EnemyScaling();
    }

    private void EnemyScaling()
    {
        MaxHP = (int)Math.Pow(Level, 2) + MaxHP;
        HP = MaxHP;
        Damage = (int)UnityEngine.Random.Range((0.2f * (float)Math.Pow(Level, 1.85) + baseDamage) * 0.9f, (0.2f * (float)Math.Pow(Level, 1.85) + baseDamage) * 1.1f);
        if (Level == 0) Damage = 0;
    }

    private ItemObject AddItemRarity(CombatItem item)
    {
        int number = UnityEngine.Random.Range(0, 101);

        if (number > 97)
        {
            item.rarity = ItemRarity.Legendary;
        }
        else if (number > 93)
        {
            item.rarity = ItemRarity.Epic;

        }
        else if (number > 90)
        {
            item.rarity = ItemRarity.Rare;
        }
        else if (number > 70)
        {
            item.rarity = ItemRarity.Uncommon;
        }
        else
        {
            item.rarity = ItemRarity.Common;
        }
        item.AddWeaponRarityModifier();
        return item;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RetreatTime = 0;
        OnCollisionEnter?.Invoke(collision);
    }

    public override void Load()
    {

    }

    public override void Save()
    {

    }

    public override void OnEnable()
    {

    }

    public override void OnDisable()
    {

    }

    [Serializable]
    private class ListItem
    {
        public ItemObject Item;
        public int Dropchance;
        public int MaxAmount;
    }
}