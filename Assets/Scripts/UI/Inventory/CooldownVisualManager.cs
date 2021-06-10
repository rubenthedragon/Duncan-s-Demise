using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CooldownVisualManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private Image imageCooldown;
    private static float cooldown;
    private InventoryInterface inventoryInterface;
    private static float currentFillAmount;
    bool cooldownStarted;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inventoryInterface = GetComponentInParent<InventoryInterface>();
        if (inventoryInterface?.inventory != player.hotbar) return;
        player.CooldownStarted += StartCooldown;
    }

    private void Update()
    {
        if (inventoryInterface?.inventory != player.hotbar) return;
        if (inventoryInterface?.itemsDisplayed[transform.parent.gameObject].item?.type == ItemType.CombatItem)
        {
            if (!cooldownStarted && player.CombatCooldownTimer < player.CombatCooldownTime)
            { 
                imageCooldown.fillAmount = currentFillAmount;
                cooldownStarted = true;
            }
        }
        else
        {
            imageCooldown.fillAmount = 0;
            cooldownStarted = false;
        }
        if (!cooldownStarted) return;
        if (player.CombatCooldownTimer < player.CombatCooldownTime)
        {
            currentFillAmount = imageCooldown.fillAmount -= 1f / cooldown * Time.deltaTime;
        }
        else
        {
            imageCooldown.fillAmount = 0;
            cooldownStarted = false;
        }
    }

    private void StartCooldown(float waitTime)
    {
        if (inventoryInterface?.itemsDisplayed[transform.parent.gameObject].item?.type != ItemType.CombatItem) return;
        cooldown = waitTime;
        cooldownStarted = true;
        currentFillAmount = imageCooldown.fillAmount = 1;
    }
}
