using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Audioplayer = null;
    [field: SerializeField] private AudioSource Music;
    [field: SerializeField] private AudioSource SFX;
    public float musicVolume { get; private set; } = 1;
    private IEnumerator fadeIn;
    private IEnumerator fadeOut;

    private void Awake()
    {
        if (Audioplayer == null)
        {
            DontDestroyOnLoad(this);
            Audioplayer = this;
        }
        else if (Audioplayer != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, float Volume = 0)
    {
        if(clip != null)
        {
            if (Volume == 0)
            {
                musicVolume = 1;
                if (Music.volume > musicVolume)
                {
                    Audioplayer.Music.volume = musicVolume;
                }
                fadeIn = FadeIn(Music, 0.5f, clip);
                if (fadeOut != null)
                    StopCoroutine(fadeOut);
                StartCoroutine(fadeIn);
            }
            else
            {
                musicVolume = Volume;
                if (Music.volume > musicVolume)
                {
                    Audioplayer.Music.volume = musicVolume;
                }
                fadeIn = FadeIn(Music, 0.5f, clip);
                if(fadeOut != null)
                    StopCoroutine(fadeOut);
                StartCoroutine(fadeIn);
            }
        }
    }

    public void StopMusic()
    {
        if(Music.volume == musicVolume)
        {
            fadeOut = FadeOut(Music, 0.5f);
            if (fadeIn != null)
                StopCoroutine(fadeIn);
            StartCoroutine(fadeOut);
        }
    }
    
    public void PlaySFX(AudioClip clip, float Volume = 0)
    {
        if (Volume == 0)
        {
            SFX.PlayOneShot(clip);
        }
        else
        {
            SFX.PlayOneShot(clip, Volume);
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = Audioplayer.Music.volume;

        while (Audioplayer.Music.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, AudioClip newClip)
    {
        float startVolume = 1;

        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.loop = true;

        while (audioSource.volume < Audioplayer.musicVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }
}
