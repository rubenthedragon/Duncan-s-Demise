using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoal : Goal
{
    public ItemObject Item { get; set; }

    public ItemGoal(Quest quest, ItemObject item, string description, bool completed, int currentAmount, int requiredAmount, GoalType goalType)
    {
        this.Quest = quest;
        this.Item = item;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.GoalType = goalType;
    }

    public override void Init()
    {
        base.Init();
        GoalEventHandler.OnItemPickup += ItemPickedUp;
        GoalEventHandler.OnItemDropped += ItemDropped;

        Player player = (Player)GameObject.Find("Player").GetComponent("Player");
        if (player == null) return;
        foreach (InventorySlot slot in player.inventory.Container.items)
        {
            if (slot.item != null)
            {
                if (slot.item.Id == Item.Id)
                {

                    CurrentAmount+= slot.amount;
                    Evaluate();
                }
            }
        }

        foreach (InventorySlot slot in player.hotbar.Container.items)
        {
            if (slot.item != null)
            {
                if (slot.item.Id == Item.Id)
                {
                    CurrentAmount += slot.amount;
                    Evaluate();
                }
            }
        }
    }

    private void ItemDropped(ItemObject item, int _amount)
    {
        if (item.Id == Item.Id)
        {
            this.CurrentAmount-= _amount;
            Evaluate();
        }
    }

    private void ItemPickedUp(ItemObject item, int _amount)
    {
        if (item.Id == Item.Id)
        {
            this.CurrentAmount+= _amount;
            Evaluate();
        }
    }

    public override void OnHandIn()
    {
        Player player = (Player)GameObject.Find("Player").GetComponent("Player");
        foreach (InventorySlot slot in player.inventory.Container.items)
        {
            if (slot.item != null)
            {
                if (slot.item.Id == Item.Id)
                {
                    player.inventory.RemoveItem(slot);
                    return;
                }
            }
        }
        foreach (InventorySlot slot in player.hotbar.Container.items)
        {
            if (slot.item != null)
            {
                if (slot.item.Id == Item.Id)
                {
                    player.hotbar.RemoveItem(slot);
                    return;
                }
            }
        }
    }
}
