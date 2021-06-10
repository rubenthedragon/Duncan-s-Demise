using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSelection : MonoBehaviour
{
    public InventoryObject hotbar;

    public Player player;

    public float xStart;
    public float y;
    private int SlotSize = 70;
    public GameObject indicator;

    private int index;

    private void Start() => SelectItem(0);

    private void Update()
    {
        CheckForInput();

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SelectItem(++index < hotbar.Container.items.Length ? index : 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SelectItem(--index > -1 ? index : hotbar.Container.items.Length - 1);
        }

        ItemAtIndex(index);
    }

    public void ItemAtIndex(int i)
    {
        ItemObject item = hotbar.Container.items[i].item ? hotbar.Container.items[i].item : null;
        player.SetItemInUse(item);
    }

    public void IndicatorPosition()
    {
        Vector2 position = new Vector2(xStart + (SlotSize * index), y);
        indicator.GetComponent<RectTransform>().localPosition = position;
    }


    private void CheckForInput()
    {
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar1)) SelectItem(0);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar2)) SelectItem(1);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar3)) SelectItem(2);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar4)) SelectItem(3);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar5)) SelectItem(4);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar6)) SelectItem(5);
        else if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Hotbar7)) SelectItem(6);
    }


    private void SelectItem(int index)
    {
        this.index = index;
        ItemAtIndex(index);
        IndicatorPosition();
    }

}