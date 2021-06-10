using UnityEngine;

public class DialogOptionsContainer : MonoBehaviour
{
    [field: SerializeField] public Option[] Options { get; private set; }

    /// <summary>
    /// Shows the options to the user
    /// </summary>
    /// <param name="responses"> The options</param>
    public void ShowOptions(ResponseLayout[] responses, AudioClip acceptQuest)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            Options[i].gameObject.SetActive(true);

            Options[i].Initialize(responses[i].Text, responses[i].Dialog, responses[i].Quest, responses[i].OpenShop, acceptQuest, responses[i].KillPlayer, responses[i].GoToScene);
        }
    }

    /// <summary>
    /// Disables all options
    /// </summary>
    public void DisableOptions()
    {
        foreach (Option option in Options)
        {
            option.gameObject.SetActive(false);
        }   
    }
}
