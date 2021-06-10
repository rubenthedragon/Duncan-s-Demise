using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [field: HideInInspector] public InventoryObject ShopInventory;
    [field: HideInInspector] public InventoryObject BuyInventory;
    [field: HideInInspector] public InventoryObject SellInventory;
    [field: HideInInspector] public Player player;
    [field: SerializeField] private DynamicInventoryInterface PlayerInventoryUI;
    [field: SerializeField] private StaticInventoryInterface BuyInventoryUI;
    [field: SerializeField] private StaticInventoryInterface SellInventoryUI;
    [field: SerializeField] private StaticInventoryInterface ShopInventoryUI;
    [field: SerializeField] private PlayerGold shopInvPlayerGold;
    [field: SerializeField] private GameObject Panel;
    [field: SerializeField] private Text GoldText;
    [field: SerializeField] private AudioClip clip;
    [field: SerializeField] private Button acceptButton;
    private int goldGive;
    private int goldRecieve;

    private void Init()
    {
        player = FindObjectOfType<Player>();
        PlayerInventoryUI.player = player;
        ShopInventoryUI.player = player;
        BuyInventoryUI.player = player;
        SellInventoryUI.player = player;
        ShopInventoryUI.inventory = ShopInventory;
        ShopInventoryUI.UpdateSlotsWithoutEvent();
        BuyInventoryUI.inventory = BuyInventory;
        SellInventoryUI.inventory = SellInventory;
        goldGive = 0;
        goldRecieve = 0;
        BuyInventoryUI.NotifyOnGoldChange += OnGiveGold;
        SellInventoryUI.NotifyOnGoldChange += OnRecieveGold;
    }

    public void ToggleUI()
    {
        if (Panel.activeSelf == false)
        {
            Panel.SetActive(true);
            Init();
        }
        else
        {
            ResetItemOnShopExit();
            Panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Panel.activeSelf)
        {
            ChangeGoldText(goldRecieve, goldGive);
        }
    }

    public void ResetItemOnShopExit()
    {
        ItemSellInvToPlayerInv();
        ItemBuyInvToShopInv();
    }

    private void ItemSellInvToPlayerInv()
    {
        foreach (InventorySlot item in SellInventory.Container.items)
        {
            if (item.ID != -1)
            {
                player.inventory.AddItem(item.item, item.amount);
                SellInventory.RemoveItem(item);
            }
        }
    }

    private void ItemBuyInvToShopInv()
    {
        foreach (InventorySlot item in BuyInventory.Container.items)
        {
            if (item.ID != -1)
            {
                ShopInventoryUI.inventory.AddItem(item.item, item.amount);
                BuyInventory.RemoveItem(item);
            }
        }
    }

    public void OnAcceptClick()
    {
        AudioPlayer.Audioplayer.PlaySFX(clip);
        CheckSellInv(true);
        CheckBuyInv(true);
        shopInvPlayerGold.UpdateGoldText();
    }

    public void CheckBuyInv(bool buy)
    {
        int value = 0;
        player = FindObjectOfType<Player>();
        foreach (InventorySlot slot in BuyInventory.Container.items)
        {
            if (slot.item != null)
            {
                value += (slot.item.value * slot.amount);
            }
        }
        if (player.Gold >= value && buy)
        {
            foreach (InventorySlot slot in BuyInventory.Container.items)
            {
                if (slot.item != null)
                {
                    player.Gold -= (slot.item.value * slot.amount);
                    GoalEventHandler.ItemPickedUp(slot.item, slot.amount);
                    player.inventory.AddItem(slot.item, slot.amount);
                    BuyInventory.RemoveItem(slot);
                }
            }
            goldGive = 0;
        }
    }

    public void CheckSellInv(bool sell)
    {
        int value = 0;
        player = FindObjectOfType<Player>();
        foreach (InventorySlot slot in SellInventory.Container.items)
        {
            if (slot.item != null)
            {
                value += (slot.item.value * slot.amount);
                if (sell)
                {
                    GoalEventHandler.ItemDropped(slot.item, slot.amount);
                    ShopInventory.AddItem(slot.item, slot.amount);
                    SellInventory.RemoveItem(slot);
                }
            }
        }
        player.Gold += SellForLess(value);
        goldRecieve = 0;
    }

    private void OnRecieveGold(int value)
    {
        goldRecieve = SellForLess(value);
    }
    private void OnGiveGold(int value)
    {
        goldGive = -1*value;
    }

    private void ChangeGoldText(int give, int recieve)
    {
        int goldRecieveOrGive = give + recieve;
        if (goldRecieveOrGive < 0 )
        {
            GoldText.color = Color.red;
        }
        else
        {
            GoldText.color = Color.white;
        }
        GoldText.text = goldRecieveOrGive.ToString();
        
        if(FindObjectOfType<Player>().Gold + goldRecieveOrGive >= 0 )
        {
            acceptButton.interactable = true;
        }
        else
        {
            acceptButton.interactable = false;
        }
    }

    private int SellForLess(int value)
    {
        return value -= (value / 5);
    }
}
