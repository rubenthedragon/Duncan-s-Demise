using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory_System_Redone/Inventory")]
public class InventoryObject : ScriptableObject
{
    public int weight;
    public GameObject dropItemPrefab;
    public bool draggingAllowed = true;
    public Inventory Container;
    [field: SerializeField] public int InventoryID;
    public ItemType? activeFilterType = null;
    public bool filterActive { get {return activeFilterType != null;} }
    public void Init()
    {
        if (DataControl.control.inventories[InventoryID] != this)
        {
            DataControl.control.OnSafe += Save;
            DataControl.control.OnLoad += Load;
            DataControl.control.inventories[InventoryID] = this;
        }
    }

    public int AddItem(ItemObject _item, int _amount)
    {
        if(_amount == 0) return 0;
        if(_item.buffs != null && _item.buffs.Length > 0)
        {
            if (SetEmptySlot(_item, _amount, activeFilterType == null ? false : activeFilterType != _item.type) == null) return _amount;
            WeightChange();
            return 0;
        }
        
        for(int i = 0; i < Container.items.Length; i++)
        {
            //If slot has same item as the one given
            if(Container.items[i].ID == _item.Id)
            {            
                //If the max stacksize of the item in the slot is reached, continue.
                if(Container.items[i].amount == Container.items[i].item.stackSize)
                {
                    continue;
                }

                //If we were to add the stacks together, check if it isn't bigger than allowed stacksize
                int newAmount = Container.items[i].amount + _amount;
                if(newAmount < Container.items[i].item.stackSize)
                {
                    Container.items[i].AddAmount(_amount);
                    WeightChange();
                    return 0;
                }

                int freeSpace = Container.items[i].item.stackSize - Container.items[i].amount;
                newAmount = _amount - freeSpace;
                if(newAmount > 0)
                {
                    int leftover = 0;
                    Container.items[i].AddAmount(freeSpace);
                    if (SetEmptySlot(_item, newAmount, activeFilterType == null ? false : activeFilterType != _item.type) == null)
                        leftover = newAmount;
                    WeightChange();
                    return leftover;
                }

                Container.items[i].AddAmount(_amount);
                WeightChange();
                return 0;                
            }
        }

        if(SetEmptySlot(_item, _amount, activeFilterType == null ? false : activeFilterType != _item.type) == null) return _amount;
        WeightChange();
        return 0;
    }

    /// <summary>
    /// Find next empty slot and fill it with the given item
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_amount"></param>
    /// <returns></returns>
    public InventorySlot SetEmptySlot(ItemObject _item, int _amount, bool? _filteredOut = null)
    {
        for (int i = 0; i < Container.items.Length; i++)
        {
            if (Container.items[i].ID <= -1 || Container.items[i].amount == 0)
            {
                Container.items[i].UpdateSlot(_item.Id, _item, _amount, _filteredOut);

                //When inventory full, add more space.
                //if (i == Container.items.Length - 1)
                //{                    
                //    Container.IncreaseSpace(sizeIncrease);
                    //IncreaseSpace();
                //}
                
                return Container.items[i];
            }
        }
        //set up functionality for when slots are full.
        return null;
    }
    public void Sort(Func<InventorySlot, int> sortingLambda, SortingOrder sortingOrder){
        switch (sortingOrder)
        {
            case(SortingOrder.Asc):
                Container.items = Container.items.OrderBy(sortingLambda).ToArray();
                break;
            case(SortingOrder.Desc):
                Container.items = Container.items.OrderByDescending(sortingLambda).ToArray();
                break;
        }
        for (int i = 0; i < Container.items.Length; i++)
        {
            Dictionary<GameObject, InventorySlot> itemsDisplayed = Container.items[i].parent.itemsDisplayed;
            itemsDisplayed[itemsDisplayed.Keys.ElementAt(i)] = Container.items[i];
        }
    }

    public void SwapItem(InventorySlot _slot1, InventorySlot _slot2)
    {
        if (!_slot1.CheckSlotRestrictions(_slot2.item) || !_slot2.CheckSlotRestrictions(_slot1.item)) return;
        InventorySlot temp = new InventorySlot(_slot2.ID, _slot2.item, _slot2.amount, _slot2.filteredOut);
        InventoryObject s1Inv = _slot1.parent.inventory;
        InventoryObject s2Inv = _slot2.parent.inventory;
        _slot2.UpdateSlot(_slot1.ID, _slot1.item, _slot1.amount, s2Inv.filterActive ? s2Inv.activeFilterType != _slot1.item?.type : false);
        _slot1.UpdateSlot(temp.ID, temp.item, temp.amount, s1Inv.filterActive ? s1Inv.activeFilterType != temp.item?.type : false);
        s1Inv.WeightChange();
        s2Inv.WeightChange();
    }

    public void SplitItem(InventorySlot _slot)
    {

        int splitAmount = _slot.amount/2;
        if(SetEmptySlot(_slot.item, splitAmount, _slot.filteredOut) != null)
        {
            _slot.UpdateSlot(_slot.ID, _slot.item, (_slot.amount - splitAmount));
        }        
    }

    public void SplitItem(InventorySlot _slot, int amount)
    {
        if (SetEmptySlot(_slot.item, amount, _slot.filteredOut) != null)
        {
            _slot.UpdateSlot(_slot.ID, _slot.item, (_slot.amount - amount));
        }
    }

    public void MergeItem(InventorySlot startSlot, InventorySlot destSlot)
    {
        if(destSlot.amount == destSlot.item.stackSize) return;
        
        int newAmount = startSlot.amount + destSlot.amount;
        if(newAmount <= startSlot.item.stackSize)
        {
            destSlot.AddAmount(startSlot.amount);
            startSlot.UpdateSlot(-1, null, 0, filterActive);
            return;
        }

        int freeSpace = destSlot.item.stackSize - destSlot.amount;
        newAmount = startSlot.amount - freeSpace;
        if(newAmount > 0)
        {
            destSlot.AddAmount(freeSpace);            
            startSlot.UpdateSlot(-1, null, 0, filterActive);
            SetEmptySlot(destSlot.item, newAmount, destSlot.filteredOut);
            return;
        }

        destSlot.AddAmount(startSlot.amount);
        return;
    }

    public void RemoveItem(InventorySlot _slot)
    {   
        _slot.UpdateSlot(-1, null, 0, filterActive);
        WeightChange();
    }

    public void WeightChange()
    {
        weight = 0;

        for (int i = 0; i < Container.items.Length; i++)
        {
            weight += Container.items[i].weight;
        }
    }

    public void DropItem(InventorySlot _slot , Vector2 position)
    {
        if (_slot.item == null) return;
        position.y -= 1;
        var obj = Instantiate(dropItemPrefab, position, Quaternion.identity);

        obj.GetComponent<GroundItem>().Item = _slot.item;
        obj.GetComponent<GroundItem>().Amount = _slot.amount;
        GoalEventHandler.ItemDropped(obj.GetComponent<GroundItem>().Item, _slot.amount);
    }

    /*
    public void IncreaseSpace()
    {
        InventorySlot_Redone[] newSize = new InventorySlot_Redone[Container.items.Length + sizeIncrease];
        Container.items.CopyTo(newSize,0);
        Container.items = newSize;
    }

    public void DecreaseSpace()
    {
        InventorySlot_Redone[] newSize = new InventorySlot_Redone[Container.items.Length - sizeIncrease];
        Container.items.CopyTo(newSize,0);
        Container.items = newSize;
    }
    */

    public void Save()
    {
        
    }

    public void Load()
    {

    }
}