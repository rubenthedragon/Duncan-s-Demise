using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class StaticInventoryInterface : InventoryInterface
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.items.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter,    delegate { OnEnter(obj);        } );
            AddEvent(obj, EventTriggerType.PointerExit,     delegate { OnExit(obj);         } );
            AddEvent(obj, EventTriggerType.BeginDrag,       delegate { OnDragStart(obj);    } );
            AddEvent(obj, EventTriggerType.EndDrag,         delegate { OnDragEnd(obj);      } );
            AddEvent(obj, EventTriggerType.Drag,            delegate { OnDrag(obj);         } );
            AddEvent(obj, EventTriggerType.PointerClick,    delegate { OnPointerClick(obj); } );

            itemsDisplayed.Add(obj, inventory.Container.items[i]);
        }
    }

    public void UpdateSlotsWithoutEvent()
    {
        itemsDisplayed = null;
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.Container.items.Length; i++)
        {
            var obj = slots[i];
            
            itemsDisplayed.Add(obj, inventory.Container.items[i]);
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
