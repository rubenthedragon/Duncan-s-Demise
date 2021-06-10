using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite LetterE;
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite open;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private ItemObject keyItem;
    [SerializeField] protected SpriteMover mover;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip cantOpenClip;
    [SerializeField] private string sceneName;
    private bool playerInRange;

    void Start()
    {
        if(renderer.sprite == null)
        renderer.sprite = closed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Player player = (Player)GameObject.Find("Player").GetComponent("Player");
            foreach (InventorySlot slot in player.inventory.Container.items)
            {
                if (slot.item != null)
                {
                    if (slot.item.Id == keyItem.Id)
                    {
                        OpenDoor();
                        break;
                    }
                }
            }

            foreach (InventorySlot slot in player.hotbar.Container.items)
            {
                if (slot.item != null)
                {
                    if (slot.item.Id == keyItem.Id)
                    {
                        OpenDoor();
                        break;
                    }
                }
            }
            AudioPlayer.Audioplayer.PlaySFX(cantOpenClip);
        }
    }

    private void OpenDoor()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        renderer.sprite = open;
        if(openClip != null)
        {
            AudioPlayer.Audioplayer.PlaySFX(openClip);
        }
        if(this != null && sceneName != null && sceneName.Length > 2)
        {
            DataControl.control.Save();
            DataControl.control.LoadNextScene(sceneName);
        }
    }

    /// <summary>
    /// If the player is in range set the playerInRange on true
    /// </summary>
    /// <param name="collision">Collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            playerInRange = true;
            SetMarker();
        }
    }

    /// <summary>
    /// If the player is out of range set the playerInRange on false
    /// </summary>
    /// <param name="collision">Collision</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            playerInRange = false;
            SetMarker();
        }
    }

    protected virtual void SetMarker()
    {
        if (playerInRange)
        {
            mover.StartRoutine(LetterE);
        }
        else
        {
            mover.StopRoutine();
        }
    }
}
