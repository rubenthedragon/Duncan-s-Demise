using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class DynamicInventoryInterface : InventoryInterface
{
    public GameObject itemSlotPrefab;
    
    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.Container.items.Length; i++)
        {            
            InventorySlot slot = inventory.Container.items[i];
            var obj = Instantiate(itemSlotPrefab, Vector2.zero, Quaternion.identity, transform);
            if(slot.amount != 0)
            {
                obj.GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
            }
            else
            {
                obj.GetComponentInChildren<Text>().text = "";
            }
            
            //create Events for dragging the items. (Used for swapping items in inventory)
            AddEvent(obj, EventTriggerType.PointerEnter,    delegate { OnEnter(obj);        } );
            AddEvent(obj, EventTriggerType.PointerExit,     delegate { OnExit(obj);         } );
            AddEvent(obj, EventTriggerType.BeginDrag,       delegate { OnDragStart(obj);    } );
            AddEvent(obj, EventTriggerType.EndDrag,         delegate { OnDragEnd(obj);      } );
            AddEvent(obj, EventTriggerType.Drag,            delegate { OnDrag(obj);         } );
            AddEvent(obj, EventTriggerType.PointerClick,    delegate { OnPointerClick(obj); } );

            itemsDisplayed.Add(obj, slot);
        }
    }


    public override void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;

        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
}
