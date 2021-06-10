using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public InventoryInterface parent;
    public int ID = -1;
    public ItemObject item;
    public int amount;
    public bool filteredOut;
    public int weight;
    public int value;
    public ItemType ItemType;
    public ItemArmorSubType armorSubType;
    public bool hasRestriction;
    public bool isWhitelist = true;

    public InventorySlot()
    {
        ResetSlot();
    }

    public void ResetSlot()
    {
        ID = -1;
        item = null;
        amount = 0;
        weight = 0;
        value = 0;
        hasRestriction = false;
    }

    public InventorySlot(int _id, ItemObject _item, int _amount, bool _filteredOut = false)
    {
        ID = _id;
        item = _item;
        amount = _amount;
        filteredOut = _filteredOut;
        if(_item == null)
        {
            weight = 0;
            value = 0;
            return;
        }
        weight = _amount * _item.weight;
        value = _amount * _item.value;
    }

    public void AddValue()
    {
        switch (item.rarity)
        {
            case ItemRarity.Common:
                item.value = item.baseValue;
                break;
            case ItemRarity.Uncommon:
                item.value = Mathf.RoundToInt(item.baseValue * 1.15f);
                break;
            case ItemRarity.Rare:
                item.value = Mathf.RoundToInt(item.baseValue * 1.3f);
                break;
            case ItemRarity.Epic:
                item.value = Mathf.RoundToInt(item.baseValue * 1.45f);
                break;
            case ItemRarity.Legendary:
                item.value = Mathf.RoundToInt(item.baseValue * 1.75f);
                break;
            default:
                break;
        }
    }

    public void UpdateSlot(int _id, ItemObject _item, int _amount, bool? _filteredOut = null)
    {
        ID = _id;
        item = _item;
        amount = _amount;
        filteredOut = _filteredOut ?? filteredOut;
        if(_item == null)
        {
            weight = 0;
            value = 0;
            return;
        }
        weight = _amount * _item.weight;
        AddValue();
        value = _amount * _item.value;                
    }

    public void AddAmount(int addAmount)
    {
        amount += addAmount;        
        weight = amount * item.weight;
        value = amount * item.value;
    }

    public bool CheckSlotRestrictions(ItemObject item)
    {
        if (item == null || !hasRestriction) return true;
        bool isRestricted = ItemType == ItemType.Armor ? item.type == ItemType && ((EquipmentObject)item).armorSubType == armorSubType : item.type == ItemType;
        return isWhitelist ? isRestricted : !isRestricted;
    }
}