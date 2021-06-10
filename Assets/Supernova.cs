using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Supernova : MonoBehaviour
{
    private bool explosion;
    private bool cracked;
    private bool blackOut;
    public SpriteRenderer planet;
    public SpriteRenderer whiteOut;
    public SpriteRenderer crackedPlanet;
    public float xSizePlanet;
    public float ySizePlanet;
    public float opacityWhiteout;
    public float fadeToBlack;

    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float animationTime;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        Time.timeScale = 1;
        AudioPlayer.Audioplayer.StopMusic();
        StartCoroutine(WaitForAnimation());
    }
    public void Update()
    {
        Vector2 newPlanetSize = new Vector2(xSizePlanet, ySizePlanet);
        planet.size = newPlanetSize;
        if (cracked)
        {
            planet.color = new Color(1f, 1f, 1f, 0);
            crackedPlanet.size = newPlanetSize;
        }
        if (explosion && !blackOut)
        {
            whiteOut.color = new Color(1f, 1f, 1f, opacityWhiteout);
        }
        if (blackOut)
        {
            whiteOut.color = new Color(fadeToBlack, fadeToBlack, fadeToBlack, 1);
        }
    }
    /// <summary>
    /// Used in unity animator as animation event
    /// </summary>
    private void Exploding()
    {
        explosion = true;
    }

    /// <summary>
    /// Used in unity animator as animation event
    /// </summary>
    private void Cracking()
    {
        cracked = true;
    }

    /// <summary>
    /// Used in unity animator as animation event
    /// </summary>
    private void BackingOut()
    {
        blackOut = true;
    }

    private IEnumerator WaitForAnimation()
    {
        AudioPlayer.Audioplayer.PlaySFX(explosionSound, 0.2f);
        yield return new WaitForSeconds(animationTime);
        NextScene();
    }


    private void NextScene()
    {
        if (DataControl.control != null)
        {
            Destroy(DataControl.control.gameObject);
        }
        SceneManager.LoadScene(nextSceneName);
    }
}