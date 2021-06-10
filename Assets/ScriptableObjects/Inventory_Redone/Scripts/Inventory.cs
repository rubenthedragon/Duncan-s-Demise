using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{    
    public InventorySlot[] items = new InventorySlot[49];

    public void IncreaseSpace(int amount)
    {
        InventorySlot[] newSize = new InventorySlot[items.Length + amount];
        items.CopyTo(newSize,0);
        items = newSize;
    }

    public void DecreaseSpace(int amount)
    {
        InventorySlot[] newSize = new InventorySlot[items.Length - amount];
        items.CopyTo(newSize,0);
        items = newSize;
    }
}
