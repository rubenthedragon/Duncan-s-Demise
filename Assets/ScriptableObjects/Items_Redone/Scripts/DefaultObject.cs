using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory_System_Redone/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake() 
    {
        type = ItemType.Generic;
    }

    public override void Use(Player player)
    {
        
    }
}
