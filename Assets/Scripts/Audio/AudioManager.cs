using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField, Range(0.0001f, 1)] private float defaultMusicVolume;
    [SerializeField, Range(0.0001f, 1)] private float defaultSFXVolume;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Text musicPercentageText;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Text SFXPercentageText;

    private void Start() => ResetAudioSettings();

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20f);
        UpdateText(musicPercentageText, musicSlider);
    }

    public void SetSFXVolume(float sliderValue)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20f);
        UpdateText(SFXPercentageText, SFXSlider);
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.Save();
    }

    public void ResetAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        float SFXVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);

        SetMusicVolume(musicVolume);
        SetSFXVolume(SFXVolume);

        musicSlider.value = musicVolume;
        SFXSlider.value = SFXVolume;
    }

    private void UpdateText(Text audioText, Slider audioSlider)
    {
        audioText.text = Mathf.RoundToInt(audioSlider.value / audioSlider.maxValue * 100).ToString() + "%";
    }
}