using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public int stackSize;
    public int weight;
    public int baseValue;
    [HideInInspector] public int value;
    public ItemBuff[] buffs;
    public ItemRarity rarity;

    public abstract void Use(Player player);

    public virtual string GetAdditionalInfo()
    {
        return "";
    }
    
}