using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Option : MonoBehaviour
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private Quest quest;
    [SerializeField] private Text optionText;
    private DialogUI dialogUI;
    private ShopUI shopUI;
    [SerializeField] private bool openShop;
    [SerializeField] private AudioClip acceptQuest;
    [SerializeField] private bool killPlayer;
    [SerializeField] private string goToScene;

    /// <summary>
    /// Initializes the option
    /// </summary>
    /// <param name="text"> The text to display</param>
    /// <param name="dialog"> The response associated with the option</param>
    public void Initialize(string text, Dialog dialog, Quest _quest, bool _openShop, AudioClip clip, bool _killPlayer, string _goToScene)
    {
        acceptQuest = clip;
        dialogUI = GameObject.Find("DialogUI").GetComponent<DialogUI>();
        optionText.text = text;
        shopUI = GameObject.Find("StoreUI").GetComponent<ShopUI>();
        openShop = _openShop;
        this.dialog = dialog;
        quest = _quest;
        killPlayer = _killPlayer;
        goToScene = _goToScene;
    }

    /// <summary>
    /// Registers the button click event of the Option
    /// </summary>
    public void OnClick()
    {
        if(quest != null)
        {
            if (GameObject.Find("Player").GetComponent<Player>())
            {
                Player player = GameObject.Find("Player").GetComponent<Player>();
                if (player.QuestList.Contains(quest))
                {
                    if (quest.IsHandedIN)
                    {
                        dialogUI.EnterOption(dialog);
                        return;
                    }
                    else if (quest.Completed)
                    {
                        quest.GiveReward();
                        player.QuestProgressionMade($"Quest handed in: '{quest.Name}'");
                    }
                    else
                    {
                        dialogUI.EnterOption(dialog);
                        return;
                    }
                }
                else
                {
                    quest.Init();
                    player.QuestList.Add(quest);
                    player.QuestProgressionMade($"Quest accepted: '{quest.Name}'");
                    AudioPlayer.Audioplayer.PlaySFX(acceptQuest, 0.8f);
                }
            }
        }
        if (openShop)
        {
            shopUI.ToggleUI();
        }
        if (killPlayer)
        {
            FindObjectOfType<Player>().HP = 0;
        }
        if(this != null && goToScene != null && goToScene.Length > 2)
        {
            if(DataControl.control)
            DataControl.control.Save();
            StartCoroutine(FindObjectOfType<LoadingScreenUI>().IncreaseAlpha(goToScene));
            return;
        }

        dialogUI.EnterOption(dialog);
    }
}