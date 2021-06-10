using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcObject : MonoBehaviour
{
    [SerializeField] private Sprite LetterE;
    [SerializeField] protected SpriteMover mover;
    protected Dialog dialog;
    [SerializeField] protected List<Dialog> dialogs;
    protected int currentDialogNr;
    protected GameObject dialogUI;
    protected bool playerInRange;

    /// <summary>
    /// Set the npc object up with the data from the dialog
    /// </summary>
    protected virtual void Start()
    {
        //This is to set the base of the npc as hitbox
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x + 0.08f, 0.5f);
        this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.5f);
        this.gameObject.GetComponent<CircleCollider2D>().offset = new Vector2(0, -0.5f);
        this.gameObject.GetComponent<CircleCollider2D>().radius = 0.2f;

        if(dialogs.Count > 0)
        {
            dialog = dialogs[0];
            currentDialogNr = 0;
        }

        dialogUI = GameObject.Find("DialogUI");
    }

    public virtual void NextDialog(int dialogNr = -1)
    {
        if(dialogNr >= 0)
        {
            currentDialogNr = dialogNr;
            dialog = dialogs[currentDialogNr];
        }
        else if (dialogs.Count > currentDialogNr+1)
        {
            currentDialogNr++;
            dialog = dialogs[currentDialogNr];
        }
    }

    /// <summary>
    /// Mash E to interact with the npc
    /// </summary>
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Check();
            if(dialog != null)
            {
                dialogUI.GetComponent<DialogUI>().StartDialog(dialog);
            }
        }
    }

    protected virtual void Check()
    {

    }

    protected virtual void SetMarker()
    {
        if (playerInRange && LetterE != null)
        {
            mover.StartRoutine(LetterE);
        }
        else if (LetterE != null)
        {
            mover.StopRoutine();
        }
    }

    /// <summary>
    /// If the player is in range set the playerInRange on true
    /// </summary>
    /// <param name="collision">Collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
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
            GameObject.Find("DialogUI").GetComponent<DialogUI>().StopDialog();
            if (GameObject.Find("StoreUI").transform.childCount > 0 && GameObject.Find("StoreUI").transform.GetChild(0).gameObject.activeSelf)
            {
                GameObject.Find("StoreUI").GetComponent<ShopUI>().ResetItemOnShopExit();
                GameObject.Find("StoreUI").transform.GetChild(0).gameObject.SetActive(false);
            }
            SetMarker();
        }
    }
}
