using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField]
    private Text resolutionText;

    private Resolution[] resolutions;

    private int currentResolutionIndex;
    private int currentFullScreenModeIndex;

    [SerializeField]
    private Button[] fullScreenModeButtons;

    private void Start()
    {
        resolutions = Screen.resolutions;
        LoadGraphicSettings();
    }

    private void SetResolutionText(Resolution resolution)
    {
        resolutionText.text = resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz";
    }

    public void SetNextResolution()
    {
        if (resolutions.Length < 1 || currentResolutionIndex + 1 >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }
        else
        {
            currentResolutionIndex++;
        }

        SetResolutionText(resolutions[currentResolutionIndex]);
    }

    public void SetPreviousResolution()
    {
        if (resolutions.Length < 1)
        {
            currentResolutionIndex = 0;
        }
        else if (currentResolutionIndex - 1 < 0)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }
        else
        {
            currentResolutionIndex--;
        }

        SetResolutionText(resolutions[currentResolutionIndex]);
    }

    public void SetFullScreenModeIndex(int index)
    {
        currentFullScreenModeIndex = index;

        for(int i = 0; i < fullScreenModeButtons.Length; i++)
        {
            fullScreenModeButtons[i].interactable = i != currentFullScreenModeIndex;
        }
    }

    private void LoadGraphicSettings()
    {
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        currentFullScreenModeIndex = PlayerPrefs.GetInt("FullScreenModeIndex", 0);

        fullScreenModeButtons[currentFullScreenModeIndex].interactable = false;

        SetResolutionText(resolutions[currentResolutionIndex]);

        ApplyGraphicSettings();
    }

    private void SaveGraphicSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        PlayerPrefs.SetInt("FullScreenModeIndex", currentFullScreenModeIndex);
    }

    public void ApplyGraphicSettings()
    {
        Screen.SetResolution(
           resolutions[currentResolutionIndex].width,
           resolutions[currentResolutionIndex].height,
            GetFullScreenModeFromIndex(currentFullScreenModeIndex)
            );

        SaveGraphicSettings();
    }

    private FullScreenMode GetFullScreenModeFromIndex(int index)
    {
        switch (index)
        {
            case 0:
                return FullScreenMode.ExclusiveFullScreen;
            case 1:
                return FullScreenMode.FullScreenWindow;
            default:
                return FullScreenMode.Windowed;
        }
    }
}