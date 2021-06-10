using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class Player : Entity, IOnLoadAndSave
{
    public MouseItem mouseItem = new MouseItem();

    [field: SerializeField] public Orbit Orbit { get; private set; }
    
    public float CombatCooldownTime { get; private set; }
    public float CombatCooldownTimer { get; private set; }
    
    public InventoryObject inventory;
    public InventoryObject hotbar;
    public InventoryObject equipment;
    private ItemObject itemInUse;
    private ItemObject currentItemObject;
    public int CarryStrength = 100;
    public int Gold { get; set; } = 5;

    private Animator animator;
    private Vector2 movement;

    public event Action<string> QuestProgressChanged;
    public event Action<int> StatPointsChanged;
    public event Action<int> EXPChanged;
    public event Action<int> LevelChanged;
    public event Action<string> StatsChanged;
    public event Action HasDied;
    public event Action<float> CooldownStarted;

    public int CurrentExp { get; private set; }
    public int StatPoints { get; set; }
    [field: HideInInspector] public int Vitality { get; set; } = 5;
    [field: HideInInspector] public int Wisdom { get; set; } = 5;
    [field: HideInInspector] public int Strength { get; set; } = 8;
    [field: HideInInspector] public int Dexterity { get; set; } = 4;
    [field: HideInInspector] public int Intelligence { get; set; } = 3;

    public int Defense;

    [field: SerializeField] public List<Quest> QuestList = new List<Quest>();
    [field: SerializeField] private AudioClip onHitRecieved;
    [field: SerializeField] private AudioClip onLevelUp;

    private bool canUseItems = true;

    private void Start()
    {
        if (hotbar.Container.items[0] != null)
        {
            itemInUse = hotbar.Container.items[0].item;
        }

        animator = GetComponent<Animator>();
    }

    public override void OnEnable()
    {
        DataControl.control.OnLoad += Load;
        DataControl.control.OnSafe += Save;
        DataControl.control.OnInventoryLoad += InventoryLoad;
        DataControl.control.OnInventorySave += InventorySave;

        inventory.Init();
        hotbar.Init();
        equipment.Init();

        UpdateMaxHP();
        UpdateMaxMana();
        ChangeHealth(MaxHP);
        ChangeMana(MaxMana);
    }

    public override void OnDisable()
    {
        DataControl.control.OnLoad -= Load;
        DataControl.control.OnSafe -= Save;
        DataControl.control.OnInventoryLoad -= InventoryLoad;
        DataControl.control.OnInventorySave -= InventorySave;
    }

    private void Update()
    {
        Die();

        RegisterMovement();

        UpdateDefense();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            UseItem();
        }

        CombatCooldownTimer += Time.deltaTime;
        CombatCooldownTimer = CombatCooldownTimer > CombatCooldownTime ? CombatCooldownTime : CombatCooldownTimer;
    }

    private void UpdateDefense()
    {
        int value = 0;
        foreach (InventorySlot slot in equipment.Container.items)
        {
            if (slot.item == null) continue;
            if (slot.item.buffs == null) continue;
            foreach (ItemBuff buff in slot.item.buffs)
            {
                if (buff.stat == Stats.Defense)
                {
                    value += buff.Value;
                }
            }
        }
        Defense = value;
    }

    public void ReceiveDamage(int value, Enemy enemy)
    {
        if (!HasInvincFrames)
        {
            AudioPlayer.Audioplayer.PlaySFX(onHitRecieved, 0.1f);
        }
        float damageReduction = Defense / (Defense + 700f + (85f * enemy.Level));
        ChangeHealth(Mathf.RoundToInt(value * (1 - damageReduction)));
    }

    private void FixedUpdate() => Move();


    private void RegisterMovement()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
        }
    }

    private void Move()
    {
        if (IsExperiencingKnockback) return;
        int weightModifier = 1;
        if(inventory.weight + hotbar.weight + equipment.weight > (2*CarryStrength))
        {
            HP = 0;
            Die();
        }
        if(inventory.weight + hotbar.weight + equipment.weight > CarryStrength)
        {
            weightModifier = 2;
        }
        Rb2d.MovePosition(Rb2d.position + movement * (MovementSpeed/weightModifier) * Time.fixedDeltaTime);
    }

    private void Die()
    {
        if (HP > 0) return;
        HasDied?.Invoke();
    }

    /// <summary>
    /// Add exp to the players total exp and change the UI for the exp and level
    /// </summary>
    /// <param name="exp">exp to add</param>
    public void AddExp(int exp)
    {
        CurrentExp += exp;

        if (CurrentExp >= ExpToNext(Level))
        {
            while (CurrentExp >= ExpToNext(Level))
            {
                AudioPlayer.Audioplayer.PlaySFX(onLevelUp, 0.3f);
                LevelUp();
            }
        }
        else
        {
            EXPChanged?.Invoke(CurrentExp);
        }
    }

    private void LevelUp()
    {
        CurrentExp -= ExpToNext(Level);
        Level++;
        StatsChanged?.Invoke("Stat Points +3");
        LevelChanged?.Invoke(Level);
        StatPoints += 3;
        StatPointsChanged?.Invoke(StatPoints);
    }

    public void IncreaseVitality()
    {
        if (StatPoints > 0)
        {
            Vitality += 1;
            StatPoints -= 1;
            UpdateMaxHP();
            ChangeHealth(MaxHP);
        }
        StatPointsChanged?.Invoke(StatPoints);
    }

    public void IncreaseWisdom()
    {
        if (StatPoints > 0)
        {
            Wisdom += 1;
            StatPoints -= 1;
            UpdateMaxMana();
            ChangeMana(MaxMana);
        }
        StatPointsChanged?.Invoke(StatPoints);
    }

    public void IncreaseStrength()
    {
        if (StatPoints > 0)
        {
            Strength += 1;
            StatPoints -= 1;
        }
        StatPointsChanged?.Invoke(StatPoints);
    }

    public void IncreaseDexterity()
    {
        if (StatPoints > 0)
        {
            Dexterity += 1;
            StatPoints -= 1;
        }
        StatPointsChanged?.Invoke(StatPoints);
    }

    public void IncreaseIntelligence()
    {
        if (StatPoints > 0)
        {
            Intelligence += 1;
            StatPoints -= 1;
        }
        StatPointsChanged?.Invoke(StatPoints);
    }

    /// <summary>
    /// Calculate the exp needed for the next level
    /// </summary>
    /// <param name="level">the current level</param>
    /// <returns>exp needed for the next level</returns>
    public int ExpToNext(int level)
    {
        return 10 * level * level + (90 * level);
    }

    public void UseItem()
    {
        if (!canUseItems || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (itemInUse == null)
        {
            return;
        }

        if (itemInUse.type == ItemType.CombatItem)
        {
            if (!CombatCooldown())
            {
                itemInUse.Use(this);
                return;
            }
            return;
        }

        itemInUse.Use(this);
    }

    public void StartCombatCooldown(float waitTime)
    {
        CombatCooldownTime = waitTime;
        CombatCooldownTimer = 0;
        CooldownStarted?.Invoke(waitTime);
    }

    private void UpdateMaxHP()
    {
        MaxHP = Vitality * 20;
    }

    private void UpdateMaxMana()
    {
        MaxMana = Wisdom * 20;
    }

    private bool CombatCooldown()
    {
        if (CombatCooldownTimer >= CombatCooldownTime)
        {
            return false;
        }

        return true;
    }

    public void SetItemInUse(ItemObject _item)
    {
        itemInUse = _item;
    }

    public void CanUseItems(bool active)
    {
        canUseItems = active;
    }

    public void QuestProgressionMade(string questName)
    {
        QuestProgressChanged?.Invoke(questName);
    }

    private void OnApplicationQuit()
    {
        foreach (InventorySlot slot in inventory.Container.items)
        {
            slot.UpdateSlot(-1, null, 0, false);
        }
        inventory.WeightChange();

        foreach (InventorySlot slot in hotbar.Container.items)
        {
            slot.UpdateSlot(-1, null, 0, false);
        }
        hotbar.WeightChange();

        foreach (InventorySlot slot in equipment.Container.items)
        {
            slot.UpdateSlot(-1, null, 0, false);
        }
        equipment.WeightChange();
    }

    public void InventoryLoad()
    {
        inventory = DataControl.control.inventories[inventory.InventoryID];
        hotbar = DataControl.control.inventories[hotbar.InventoryID];
        equipment = DataControl.control.inventories[equipment.InventoryID];
    }

    public override void Load()
    {
        HP = DataControl.control.Health;
        MaxHP = DataControl.control.MaxHealth;
        Mana = DataControl.control.Mana;
        MaxMana = DataControl.control.MaxMana;
        Level = DataControl.control.Level;
        CurrentExp = DataControl.control.CurrentEXP;
        StatPoints = DataControl.control.StatPoints;
        Vitality = DataControl.control.Vitality;
        Wisdom = DataControl.control.Wisdom;
        Strength = DataControl.control.Strength;
        Dexterity = DataControl.control.Dexterity;
        Intelligence = DataControl.control.Intelligence;
        Gold = DataControl.control.Gold;
        QuestList = DataControl.control.Quests;
        DataControl.control.LoadInventory(inventory.InventoryID);
        DataControl.control.LoadInventory(hotbar.InventoryID);
        DataControl.control.LoadInventory(equipment.InventoryID);

        if (DataControl.control.CheckForWorldSave())
        {
            this.gameObject.transform.position = DataControl.control.PlayerPosition;
        }
    }

    public void InventorySave()
    {
        DataControl.control.inventories[inventory.InventoryID] = inventory;
        DataControl.control.inventories[hotbar.InventoryID] = hotbar;
        DataControl.control.inventories[equipment.InventoryID] = equipment;
    }

    public override void Save()
    {
        DataControl.control.Health = HP;
        DataControl.control.MaxHealth = MaxHP;
        DataControl.control.Mana = Mana;
        DataControl.control.MaxMana = MaxMana;
        DataControl.control.Level = Level;
        DataControl.control.CurrentEXP = CurrentExp;
        DataControl.control.StatPoints = StatPoints;
        DataControl.control.Vitality = Vitality;
        DataControl.control.Wisdom = Wisdom;
        DataControl.control.Strength = Strength;
        DataControl.control.Dexterity = Dexterity;
        DataControl.control.Intelligence = Intelligence;
        DataControl.control.Gold = Gold;
        DataControl.control.PlayerPosition = this.gameObject.transform.position;
        DataControl.control.Quests = QuestList;
        DataControl.control.SaveInventory(inventory.InventoryID);
        DataControl.control.SaveInventory(hotbar.InventoryID);
        DataControl.control.SaveInventory(equipment.InventoryID);
    }
}
