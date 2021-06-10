using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketPlaceFinal : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float cameraIntroWait;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private GameObject dialogCanvas;
    [SerializeField] private Dialog dialog;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip angryCrowd;
    [SerializeField] private AudioClip hatch;
    [SerializeField] private GameObject closeHatch;
    [SerializeField] private GameObject openHatch;
    [SerializeField] private GameObject duncanShadow;
    [SerializeField] private LoadingScreenUI loadingScreenUI;
    [SerializeField] private string endCreditsSceneName;

    private bool dialogDisplayed;

    private void Start() => StartCoroutine(PlayIntro());

    private void Update()
    {
        if (!dialogDisplayed || dialogCanvas.activeSelf) return;
        HangingPart();
    }


    private IEnumerator PlayIntro()
    {
        AudioPlayer.Audioplayer.PlayMusic(music, 0.048f);
        AudioPlayer.Audioplayer.PlaySFX(angryCrowd, 0.048f);
        Time.timeScale = 1;
        yield return new WaitForSeconds(cameraIntroWait);
        dialogUI.StartDialog(dialog);
        dialogDisplayed = true;
    }

    private void HangingPart()
    {
        dialogDisplayed = false;
        closeHatch.SetActive(false);
        openHatch.SetActive(true);
        duncanShadow.SetActive(false);
        AudioPlayer.Audioplayer.PlaySFX(hatch, 1);
        StartCoroutine(loadingScreenUI.IncreaseAlpha(endCreditsSceneName));
    }

}