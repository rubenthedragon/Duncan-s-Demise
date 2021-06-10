using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScreen : MonoBehaviour
{
    [SerializeField] private bool saveGamePresent;
    [SerializeField] private GameObject warningContainer;

    [SerializeField] private int newGameSceneBuildIndex;

    private void OnEnable()
    {
        saveGamePresent = DataControl.control.CheckForSave();
        if (saveGamePresent)
        {
            warningContainer.SetActive(true);
        }
        else
        {
            LoadNewGame();
        }
    }

    public void LoadNewGame()
    {
        DataControl.control.NewGame(newGameSceneBuildIndex);
    }
}
