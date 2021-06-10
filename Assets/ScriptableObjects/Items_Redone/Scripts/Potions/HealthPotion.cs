using UnityEngine;

[CreateAssetMenu(fileName = "Potion item", menuName = "Items/Potion/HealthPotion")]
public class HealthPotion : ItemObject
{
    [field: SerializeField] public int ReplenishAmount { get; set; }
    [field: SerializeField] public AudioClip drinkSound { get; set; }

    private void Awake() => type = ItemType.Potion;

    public override void Use(Player player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.HP < player.MaxHP)
            {
                AudioPlayer.Audioplayer.PlaySFX(drinkSound);
                player.ChangeHealth(ReplenishAmount);

                for (int i = 0; i < player.hotbar.Container.items.Length; i++)
                {
                    if (player.hotbar.Container.items[i].item == this)
                    {
                        if (player.hotbar.Container.items[i].amount > 1)
                        {
                            player.hotbar.Container.items[i].amount--;
                            player.hotbar.Container.items[i].UpdateSlot(player.hotbar.Container.items[i].ID, player.hotbar.Container.items[i].item, player.hotbar.Container.items[i].amount--, false);
                            player.hotbar.WeightChange();
                        }
                        else
                        {
                            player.hotbar.RemoveItem(player.hotbar.Container.items[i]);
                        }
                    }
                }
            }
        }
    }

    public override string GetAdditionalInfo()
    {
        return string.Format("Restore HP: {0}", ReplenishAmount);
    }
}
