using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject DeathScreenContainer;
    [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] private Text deathTipText;
    [SerializeField] private string[] tips;
    [SerializeField] private AudioClip source;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();

        if (player == null) return;

        player.HasDied += ShowDeathScreen;
    }

    private void ShowDeathScreen()
    {
        InsertTip();
        DeathScreenContainer.SetActive(true);
        Time.timeScale = 0;
        AudioPlayer.Audioplayer.PlaySFX(source, 0.004f);
    }

    public void LoadLastSave()
    {
        player.ChangeHealth(player.MaxHP);
        player.ChangeMana(player.MaxMana);
        Time.timeScale = 1f;

        DataControl.control.ContinueGame();
    }
    
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        foreach (InventorySlot slot in player.inventory.Container.items)
        {
            if (slot.item != null)
            {
                player.inventory.RemoveItem(slot);
            }
        }

        foreach (InventorySlot slot in player.hotbar.Container.items)
        {
            if (slot.item != null)
            {
                player.hotbar.RemoveItem(slot);
            }
        }

        foreach (InventorySlot slot in player.equipment.Container.items)
        {
            if (slot.item != null)
            {
                player.equipment.RemoveItem(slot);
            }
        }

        SceneManager.LoadScene(mainMenuBuildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void InsertTip()
    {
        if (deathTipText.text.Equals("Tips"))
        {
            deathTipText.text = tips[UnityEngine.Random.Range(0, tips.Length - 1)];
        }
    }
}