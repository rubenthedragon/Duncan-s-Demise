using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopNpc : NpcObject, IOnLoadAndSave
{
    [field: SerializeField] public InventoryObject Inventory;
    [field: SerializeField] public InventoryObject BuyInv;
    [field: SerializeField] public InventoryObject SellInv;
    [SerializeField] private List<ListItem> inventoryList;

    // Start is called before the first frame update
    protected override void Start()
    {
        DataControl.control.OnInventoryLoad += InventoryLoad;
        DataControl.control.OnInventorySave += InventorySave;
        base.Start();
    }

    protected override void Check()
    {
        GameObject.Find("StoreUI").GetComponent<ShopUI>().ShopInventory = Inventory;
        Transform[] children = GameObject.Find("StoreUI").GetComponentsInChildren<Transform>(true);
        children.Where(t => t.name == "PanelShopInventory").FirstOrDefault().GetComponentInChildren<InventorySorting>(true).inventory = Inventory;
        GameObject.Find("StoreUI").GetComponent<ShopUI>().BuyInventory = BuyInv;
        GameObject.Find("StoreUI").GetComponent<ShopUI>().SellInventory = SellInv;

        dialog = dialogs[0];
        if (GameObject.Find("StoreUI").transform.childCount > 0 && GameObject.Find("StoreUI").transform.GetChild(0).gameObject.activeSelf)
        {
            dialog = null;
        }
        base.Check();
    }

    private void InventorySave()
    {
        DataControl.control.inventories[Inventory.InventoryID] = Inventory;
    }

    private void InventoryLoad()
    {
        if (DataControl.control.CheckForInventorySave(Inventory.InventoryID))
        {
            Inventory = DataControl.control.inventories[Inventory.InventoryID];
        }
        else
        {
            foreach (InventorySlot slot in Inventory.Container.items)
            {
                slot.ResetSlot();
            }
            foreach (ListItem litem in inventoryList)
            {
                Inventory.AddItem(litem.Item, litem.Amount);
            }
        }
    }

    public void OnEnable()
    {
        DataControl.control.OnInventoryLoad += InventoryLoad;
        DataControl.control.OnInventorySave += InventorySave;

/*        Inventory.Init();
        BuyInv.Init();
        SellInv.Init();*/
    }

    public void OnDisable()
    {
        DataControl.control.OnInventoryLoad -= InventoryLoad;
        DataControl.control.OnInventorySave -= InventorySave;
    }

    public void Save()
    {
        DataControl.control.SaveInventory(Inventory.InventoryID);
    }

    public void Load()
    {
        DataControl.control.LoadInventory(Inventory.InventoryID);
    }

    [Serializable]
    private class ListItem
    {
        public ItemObject Item;
        public int Amount;
    }
}
