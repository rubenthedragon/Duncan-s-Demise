using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Item", menuName = "Inventory_System_Redone/Items/Quest")]
public class QuestObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.QuestItem;
    }

    public override void Use(Player player)
    {

    }
}
