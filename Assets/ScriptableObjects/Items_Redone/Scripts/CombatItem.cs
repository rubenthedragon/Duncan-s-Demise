using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Object", menuName = "Inventory_System_Redone/Items/CombatItem")]
public class CombatItem : ItemObject
{
    [field: SerializeField] public CombatItemObject combatItemObject;
    [field: SerializeField] public int MinDamage;
    [field: SerializeField] public int MaxDamage;

    public CombatItemObject initCombatItemObject;

    public void Awake()
    {
        type = ItemType.CombatItem;
    }

    /// <summary>
    /// Instantiates the current combatItem
    /// </summary>
    private void InstantiateCombatItem()
    {
        if (initCombatItemObject != null)
        {
            return;
        }

        initCombatItemObject = GameObject.Instantiate(combatItemObject);
        ObjectActivator.Deactivate(initCombatItemObject.gameObject);
    }

    public override void Use(Player player)
    {
        if (Input.GetMouseButtonDown(1) && player.Mana >= combatItemObject.RightAbility.ManaCost)
        {
            useRight(player);
        }

        if (Input.GetMouseButtonDown(0) && player.Mana >= combatItemObject.LeftAbility.ManaCost)
        {
            UseLeft(player);
        }
    }

    public void UseLeft(Player player)
    {
        InstantiateCombatItem();

        initCombatItemObject.MinDamage = MinDamage;
        initCombatItemObject.MaxDamage = MaxDamage;

        player.Orbit.CurrentCombatItemObject = initCombatItemObject;
        initCombatItemObject.player = player;

        player.Orbit.SetState(new OrbitCombatState(player.Orbit, Direction.Left));
        player.StartCombatCooldown(initCombatItemObject.LeftAbility.CoolDown);
    }

    public void useRight(Player player)
    {
        InstantiateCombatItem();

        initCombatItemObject.MinDamage = MinDamage;
        initCombatItemObject.MaxDamage = MaxDamage;

        player.Orbit.CurrentCombatItemObject = initCombatItemObject;
        initCombatItemObject.player = player;

        player.Orbit.SetState(new OrbitCombatState(player.Orbit, Direction.Right));
        player.StartCombatCooldown(initCombatItemObject.RightAbility.CoolDown);
    }

    public void AddWeaponRarityModifier()
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                break;
            case ItemRarity.Uncommon:
                MinDamage = Mathf.RoundToInt(MinDamage * 1.15f);
                MaxDamage = Mathf.RoundToInt(MaxDamage * 1.15f);
                break;
            case ItemRarity.Rare:
                MinDamage = Mathf.RoundToInt(MinDamage * 1.3f);
                MaxDamage = Mathf.RoundToInt(MaxDamage * 1.3f);
                break;
            case ItemRarity.Epic:
                MinDamage = Mathf.RoundToInt(MinDamage * 1.45f);
                MaxDamage = Mathf.RoundToInt(MaxDamage * 1.45f);
                break;
            case ItemRarity.Legendary:
                MinDamage = Mathf.RoundToInt(MinDamage * 1.75f);
                MaxDamage = Mathf.RoundToInt(MaxDamage * 1.75f);
                break;
            default:
                break;
        }
    }

    public override string GetAdditionalInfo()
    {
        Player player = FindObjectOfType<Player>();
        int stat;
        string type;
        switch (combatItemObject.LeftAbility.abilityType)
        {
            case AbilityType.Melee:
                stat = player.Strength;
                type = "strength";
                break;
            case AbilityType.Ranged:
                stat = player.Dexterity;
                type = "dexterity";
                break;
            case AbilityType.Magic:
                stat = player.Intelligence;
                type = "intelligence";
                break;
            default:
                stat = 0;
                type = "";
                break;
        }

        return string.Format(
            "Abilities ({1}):\n" +
            "Left: {2}\n" +
            "Damage {3}-{4}\n" +
            "Right: {5}\n" +
            "Damage {6}-{7}\n" +
            "\n" +
            "Rarity: {0}",
            rarity,
            type,
            combatItemObject.LeftAbility.GetDescription(),
            Mathf.FloorToInt(MinDamage + combatItemObject.LeftAbility.MinDamageModifier * stat),
            Mathf.CeilToInt(MaxDamage + combatItemObject.LeftAbility.MaxDamageModifier * stat),
            combatItemObject.RightAbility.GetDescription(),
            Mathf.FloorToInt(MinDamage + combatItemObject.RightAbility.MinDamageModifier * stat),
            Mathf.CeilToInt(MaxDamage + combatItemObject.RightAbility.MaxDamageModifier * stat));
    }
}
