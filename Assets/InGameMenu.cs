using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static bool canOpenPanel;
    [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] private Button loadLastSaveButton;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        loadLastSaveButton.interactable = DataControl.control.CheckForSave();
        PauseGame(true);
    }

    private void OnDisable() => PauseGame(false);

    public void PauseGame(bool pause) => Time.timeScale = pause ? 0f : 1f;
    public void LoadLastSave() => DataControl.control.ContinueGame();
    public void LoadMainMenu() 
    {
        StopAllCoroutines();
        foreach(InventorySlot slot in GameObject.Find("Player").GetComponent<Player>().inventory.Container.items)
        {
            if(slot.item != null)
            {
                GameObject.Find("Player").GetComponent<Player>().inventory.RemoveItem(slot);
            }
        }
        
        foreach(InventorySlot slot in GameObject.Find("Player").GetComponent<Player>().hotbar.Container.items)
        {
            if(slot.item != null)
            {
                GameObject.Find("Player").GetComponent<Player>().hotbar.RemoveItem(slot);
            }
        }
        
        foreach(InventorySlot slot in GameObject.Find("Player").GetComponent<Player>().equipment.Container.items)
        {
            if(slot.item != null)
            {
                GameObject.Find("Player").GetComponent<Player>().equipment.RemoveItem(slot);
            }
        }

        SceneManager.LoadScene(mainMenuBuildIndex);
    }
}
