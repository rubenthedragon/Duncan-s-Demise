using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GroundItem : MonoBehaviour, IOnLoadAndSave
{
    [field: SerializeField] public ItemObject Item { get; set; }
    [field: SerializeField] public int Amount { get; set; } = 1;

    [SerializeField] private PickUpDisplay pickUpDisplay;
    [SerializeField] private AudioClip pickupSound;

    private bool playerHasEntered;
    private Player player;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = Item.uiDisplay;
        pickUpDisplay.Display(string.Empty, ItemRarity.Common);
    }

    private void Update() => PickUpItem();

    private void PickUpItem()
    {
        if (!playerHasEntered) return;

        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.UseItem, KeyEventType.Down))
        {
            if(Amount > 0)
            {
                int emptySlotCount = player.inventory.Container.items.Sum(s => s.item == null ? 1 : 0);
                if(emptySlotCount > 0)
                {
                    int leftover = player.hotbar.AddItem(this.Item, this.Amount);
                    leftover = player.inventory.AddItem(this.Item, leftover);
                    AudioPlayer.Audioplayer.PlaySFX(pickupSound, 0.1f);
                    GoalEventHandler.ItemPickedUp(Item, Amount);
                    if(leftover != 0) this.Amount = leftover;
                    else Destroy(this.gameObject);
                }
            }
        }
    }

    public void SetItem(ItemObject item, int amount = 1)
    {
        transform.Translate(Vector3.back / 10);
        Item = item;
        Amount = amount;
        GetComponent<SpriteRenderer>().sprite = item.uiDisplay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (!player || other.isTrigger) return;

        pickUpDisplay.Display(Item.name, Item.rarity);

        this.player = player;
        playerHasEntered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (!player || other.isTrigger) return;

        pickUpDisplay.Display(string.Empty, ItemRarity.Common);
        playerHasEntered = false;
    }

    public void Save()
    {
        if (this != null)
        {
            DataControl.control.groundItems.Add(this);
        }
    }

    public void Load()
    {
        if (!DataControl.control.groundItems.Contains(this) && DataControl.control.CheckForWorldSave())
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    public void OnEnable()
    {
        DataControl.control.OnSafe += Save;
        DataControl.control.OnLoad += Load;
    }

    public void OnDisable()
    {
        DataControl.control.OnSafe -= Save;
        DataControl.control.OnLoad -= Load;
    }
}
