using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool canOpenUI = true;
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject InGameMenuUI;
    [SerializeField] private GameObject QuestUI;
    [SerializeField] private GameObject QuestBook;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject DeathScreenUI;
    [SerializeField] private GameObject MapUI;
    [SerializeField] private InventoryTabSwitcher inventoryTabSwitcher;


    [SerializeField] private GameObject LoadLastSaveContainer;
    [SerializeField] private GameObject ControlsContainer;
    [SerializeField] private GameObject SettingsContainer;
    [SerializeField] private GameObject QuitToMainMenuContainer;
    [SerializeField] private GameObject QuitGameContainer;

    [SerializeField] private Player Player;

    private Dictionary<UIType, GameObject> UIs = new Dictionary<UIType, GameObject>();

    private void Awake()
    {
        UIs.Add(UIType.Main, MainUI);
        UIs.Add(UIType.Inventory, InventoryUI);
        UIs.Add(UIType.QuestLog, QuestBook);
        UIs.Add(UIType.InGameMenu, InGameMenuUI);
        UIs.Add(UIType.Shop, ShopUI);
        UIs.Add(UIType.Map, MapUI);

        UIs.Add(UIType.LoadLastSaveContainer, LoadLastSaveContainer);
        UIs.Add(UIType.ControlsContainer, ControlsContainer);
        UIs.Add(UIType.SettingsContainer, SettingsContainer);
        UIs.Add(UIType.QuitToMainMenuContainer, QuitToMainMenuContainer);
        UIs.Add(UIType.QuitGameContainer, QuitGameContainer);

        EnableUI(UIType.Main, true);
    }

    private void EnableUI(UIType uiType, bool shouldEnable)
    {      
        if(uiType == UIType.InGameMenu)
        {
            canOpenUI = !shouldEnable;
        }
        UIs[uiType].SetActive(shouldEnable);
    }

    public void OpenInventoryUI() => OpenTab(0);
    public void OpenStatsUI() => OpenTab(1);
    public void OpenQuestLog()
    {
        if(canOpenUI)
         QuestUI.GetComponent<QuestUI>().OpenCloseQuestlog();
    }

    public void OpenInGameMenu()
    {
        EnableUI(UIType.InGameMenu, !UIs[UIType.InGameMenu].activeSelf);
        MainUI.SetActive(!UIs[UIType.InGameMenu].activeSelf);
    }

    private void CloseShop()
    {
        ShopUI.GetComponentInParent<ShopUI>().ResetItemOnShopExit();
    }

    private void Update()
    {
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Inventory))
        {
            OpenInventoryUI();
        }
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Stats))
        {
            OpenStatsUI();
        }

        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Questlog))
        {
            OpenQuestLog();
        }
        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Map))
        {
            if (canOpenUI)
            {
                EnableUI(UIType.Map, !UIs[UIType.Map].activeSelf);
            }
        }

        if (InputManager.Instance.CheckKeyPressed(InputManager.Instance.keymapping.Exitbutton))
        {
            if (UIs[UIType.Inventory].activeSelf)
            {
                EnableUI(UIType.Inventory, false);

            }
            else if(UIs[UIType.QuestLog].activeSelf)
            {
                EnableUI(UIType.QuestLog, false);
            }
            else if(UIs[UIType.Shop].activeSelf)
            {
                CloseShop();
                EnableUI(UIType.Shop, false);
            }
            else if (UIs[UIType.Map].activeSelf)
            {
                EnableUI(UIType.Map, false);
            }

            else if (UIs[UIType.LoadLastSaveContainer].activeSelf)
            {
                EnableUI(UIType.LoadLastSaveContainer, false);
            }
            else if (UIs[UIType.ControlsContainer].activeSelf)
            {
                EnableUI(UIType.ControlsContainer, false);
            }
            else if (UIs[UIType.SettingsContainer].activeSelf)
            {
                EnableUI(UIType.SettingsContainer, false);
            }
            else if (UIs[UIType.QuitToMainMenuContainer].activeSelf)
            {
                EnableUI(UIType.QuitToMainMenuContainer, false);
            }
            else if (UIs[UIType.QuitGameContainer].activeSelf)
            {
                EnableUI(UIType.QuitGameContainer, false);
            }

            else
            {
                OpenInGameMenu();
            }
        }
    }

    private void OpenTab(int index)
    {
        if (canOpenUI && !ShopUI.activeSelf)
        {
            Player.CanUseItems(true);

            if (inventoryTabSwitcher.Tabs[index].isActiveAndEnabled)
            {
                EnableUI(UIType.Inventory, false);
            }
            else
            {
                EnableUI(UIType.Inventory, true);
                inventoryTabSwitcher.SwitchToSpecificTab(inventoryTabSwitcher.Tabs[index]);
            }
        }
    }
}