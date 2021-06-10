using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public abstract class InventoryInterface : MonoBehaviour
{
    public Player player;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    public event Action<int> NotifyOnGoldChange;
    private bool Dragging;

    // Start is called before the first frame update
    public void Start()
    {
        if (inventory != null)
        {
            for (int i = 0; i < inventory.Container.items.Length; i++)
            {
                inventory.Container.items[i].parent = this;
            }
            CreateSlots();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
        if (inventory != null)
        {
            for (int i = 0; i < inventory.Container.items.Length; i++)
            {
                inventory.Container.items[i].parent = this;
            }
        }
    }


    public abstract void CreateSlots();

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            _slot.Key.GetComponent<Image>().color = _slot.Value.filteredOut ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
            //If there is an item in the slot.
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.item.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = _slot.Value.filteredOut ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<Text>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            //If there is no item in the slot.
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public virtual void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;

        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.RightClick, KeyEventType.Hold)) return;
        Dragging = true;
        GameObject mouseObject = new GameObject();
        RectTransform rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(GameObject.Find(/* Name of canvas. */ "InventoryUI").transform);

        if (itemsDisplayed[obj].ID < 0) 
        {
            Destroy(mouseObject);
            return;
        }
        Image image = mouseObject.AddComponent<Image>();
        image.sprite = itemsDisplayed[obj].item.uiDisplay;
        image.raycastTarget = false;

        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (player.mouseItem.obj == null)
        {
            Dragging = false;
            return;
        }

        if (player.mouseItem.hoverObj && !DragNotAllowed())
        {
            if (player.mouseItem.hoverItem.ID == player.mouseItem.item.ID && player.mouseItem.item.ID != -1)
            {
                if (player.mouseItem.hoverItem != player.mouseItem.item)
                {
                    inventory.MergeItem(itemsDisplayed[obj], player.mouseItem.hoverItem.parent.itemsDisplayed[player.mouseItem.hoverObj]         /*itemsDisplayed[player.mouseItem.hoverObj]*/);
                }
            }
            else
            {
                inventory.SwapItem(itemsDisplayed[obj], player.mouseItem.hoverItem.parent.itemsDisplayed[player.mouseItem.hoverObj]         /*itemsDisplayed[player.mouseItem.hoverObj]*/);
            }
            inventory.WeightChange();
            player.mouseItem.hoverItem.parent.inventory.WeightChange();
            CheckForChangeInventories();
        }
        else
        {
            if (!DragNotAllowed())
            {
                inventory.DropItem(itemsDisplayed[obj], player.transform.position);
                inventory.RemoveItem(itemsDisplayed[obj]);
                if (player.mouseItem.hoverItem != null) CheckForChangeInventories();
            }
        }

        Destroy(player.mouseItem.obj);
        player.mouseItem.item = null;
        Dragging = false;
    }

    private void CheckForChangeInventories()
    {
        if (player.mouseItem.hoverItem.parent.inventory.InventoryID == 5 || player.mouseItem.hoverItem.parent.inventory.InventoryID == 3)
        {
            player.mouseItem.hoverItem.parent.NotifyOnGoldChange?.Invoke(ChangeGold(true));
        }
        else
        {
            NotifyOnGoldChange?.Invoke(ChangeGold(false));
        }
    }
    private bool DragNotAllowed()
    {
        if (player.mouseItem.hoverItem == null)
        {
            if (inventory.draggingAllowed) return false;
            else return true;
        }
        if (inventory.InventoryID == player.mouseItem.hoverItem.parent.inventory.InventoryID) return false;
        if (inventory.InventoryID <= 3 && player.mouseItem.hoverItem.parent.inventory.InventoryID > 3) return true;
        if (inventory.InventoryID > 3 && player.mouseItem.hoverItem.parent.inventory.InventoryID <= 3) return true;
        return false;
    }

    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnPointerClick(GameObject obj)
    {
        if (Dragging) return;
        //get split amount by 1.
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Splitting, KeyEventType.Hold))
        {
            inventory.SplitItem(itemsDisplayed[obj], 1);
            return;
        }
        //Split stack by half.
        if (itemsDisplayed[obj].amount > 1)
        {
            inventory.SplitItem(itemsDisplayed[obj]);
        }
    }

    public int ChangeGold(bool wrongParent)
    {
        int value = 0;
        if (wrongParent)
        {
            foreach (InventorySlot slot in player.mouseItem.hoverItem.parent.inventory.Container.items)
            {
                if (slot.item != null)
                {
                    value += slot.value;
                }
            }
        }
        else
        {
            foreach (InventorySlot slot in inventory.Container.items)
            {
                if (slot.item != null)
                {
                    value += slot.value;
                }
            }
        }
        return value;
    }

    private void OnDisable()
    {

        if (player.mouseItem.obj != null)
        {
            Destroy(player.mouseItem.obj);
            player.mouseItem.item = null;
        }

    }

}
