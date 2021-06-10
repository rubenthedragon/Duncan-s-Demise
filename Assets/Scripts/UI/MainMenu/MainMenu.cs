using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private Vector3[] cameraPositions;
    [SerializeField] private GameObject[] subMenus;

    [SerializeField] private Button continueButton;
    [SerializeField] private AudioClip Music;


    private void Awake()
    {
        continueButton.interactable = CheckIfSaveGameIsPresent();
        AudioPlayer.Audioplayer.PlayMusic(Music, 0.04f);
        Time.timeScale = 1f;
    }

    private void Start(){
        Discord.Activity updatedActivity = DiscordRPCScript.Instance.MainActivity;
        updatedActivity.State = $"Playing Duncan's demise";
        DiscordRPCScript.Instance.UpdateMainActivity(updatedActivity);
    }

    public void OpenSubMenu(GameObject subMenu)
    {
        subMenu.SetActive(true);

        foreach (GameObject otherSub in subMenus.Where(sub => sub != subMenu).ToArray())
        {
            otherSub.SetActive(false);
        }
    }

    public void SwitchCameraPosition(int index)
    {
        if (index < 0) return;
        if (index >= cameraPositions.Length) return;

        renderCamera.transform.position = cameraPositions[index];  
    }

    private bool CheckIfSaveGameIsPresent()
    {
        return DataControl.control.CheckForSave();
    }

    public void ContinueGame()
    {
        DataControl.control.ContinueGame();  
    }
}
