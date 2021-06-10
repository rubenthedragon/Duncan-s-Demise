using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionDisplay : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    private Player player;


    private string headerDefaultText;
    private string descriptionDefaultText;


    private void Awake()
    {
        headerDefaultText = itemNameText.text;
        descriptionDefaultText = itemDescriptionText.text;
        player = GameObject.Find("Player").GetComponent<Player>();
        CheckForHover();
    }

    private void Update()
    {
        CheckForHover();
    }

    public Color ItemNameColor()
    {
        if (player.mouseItem.hoverItem == null || player.mouseItem.hoverItem.item == null) return itemNameText.color = Color.white;
        switch (player.mouseItem.hoverItem.item.rarity)
        {
            case ItemRarity.Common:
                return itemNameText.color = Color.white;
                break;
            case ItemRarity.Uncommon:
                return itemNameText.color = Color.green;
                break;
            case ItemRarity.Rare:
                return itemNameText.color = Color.blue;
                break;
            case ItemRarity.Epic:
                return itemNameText.color = Color.magenta;
                break;
            case ItemRarity.Legendary:
                return itemNameText.color = new Color32(252, 161, 3, 255);
                break;
            default:
                return itemNameText.color = Color.white;
                break;
        }
    }

    private void CheckForHover()
    {
        if (player.mouseItem.hoverItem != null)
        {
            UpdateDisplay(player.mouseItem.hoverItem);
            if (player.mouseItem.hoverItem.item != null)
            {
                SetActiveShopDescription(true, player.mouseItem.hoverItem.parent.name);
            }
            else
            {
                SetActiveShopDescription(false, player.mouseItem.hoverItem.parent.name);
            }
        }
        else
        {
            if (!transform.GetChild(0).gameObject.activeSelf) return;
            SetActiveShopDescription(false, "");
            itemNameText.color = ItemNameColor();
            itemNameText.text = headerDefaultText;
            itemDescriptionText.text = descriptionDefaultText;
        }
    }

    private void SetActiveShopDescription(bool active, string onHoverInvName)
    {
        if ((onHoverInvName.Equals("PanelPlayerInventorySlots") || (onHoverInvName.Equals("Panel_DisplayHotbarItems") || onHoverInvName.Equals("PanelSellInventory"))) && name.Equals("PanelPlayerShopItemDescription"))
        {
            transform.Find("PlayerShopItemDescription").gameObject.SetActive(active);
            return;
        }
        else if ((onHoverInvName.Equals("PanelShopInventorySlots") || onHoverInvName.Equals("PanelBuyInventory")) && name.Equals("PanelShopItemDescription"))
        {
            transform.Find("ShopItemDescription").gameObject.SetActive(active);
            return;
        }
        if (name.Equals("PanelPlayerShopItemDescription") && transform.Find("PlayerShopItemDescription").gameObject.activeSelf)
        {
            transform.Find("PlayerShopItemDescription").gameObject.SetActive(false);
            return;
        }
        if (name.Equals("PanelShopItemDescription") && transform.Find("ShopItemDescription").gameObject.activeSelf)
        {
            transform.Find("ShopItemDescription").gameObject.SetActive(false);
            return;
        }
    }

    private void UpdateDisplay(InventorySlot inventorySlot)
    {
        ItemObject item = inventorySlot.item;
        if (item)
            inventorySlot.AddValue();

        itemNameText.text = item ? item.name : headerDefaultText;
        itemNameText.color = ItemNameColor();
        itemDescriptionText.text = item ? item.description + "\n\n" + item.GetAdditionalInfo() + "\nWeight: " + item.weight + "\nBase price: " + item.value : descriptionDefaultText;
    }

}