using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private AudioClip endCreditsMusic;
    [SerializeField] private CreditsAnimation creditsAnimation;
    [SerializeField] private string nextSceneName;
    private void Start()
    {
        Time.timeScale = 1;
        AudioPlayer.Audioplayer.StopMusic();
        AudioPlayer.Audioplayer.PlayMusic(endCreditsMusic, 0.048f);
        creditsAnimation.ReachedEnd += Return;
    }


    public void Return()
    {
        if (DataControl.control != null)
        {
            Destroy(DataControl.control.gameObject);
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
