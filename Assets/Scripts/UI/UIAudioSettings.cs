using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIAudioSettings : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider voicesSlider;

    [SerializeField]
    private AudioMixer mixer;

    public void OnEnable()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        voicesSlider.value = PlayerPrefs.GetFloat("voicesVolume", 0.8f);

        SetMasterVolume(masterSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetMusicVolume(musicSlider.value);
        SetVoicesVolume(voicesSlider.value);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        voicesSlider.onValueChanged.AddListener(SetVoicesVolume);
    }

    private void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("masterVolume", value);
        mixer.SetFloat("MasterVolume", LinearToDB(value));
    }

    private void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("sfxVolume", value);
        mixer.SetFloat("SFXVolume", LinearToDB(value));
    }

    private void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("musicVolume", value);
        mixer.SetFloat("MusicVolume", LinearToDB(value));
    }

    private void SetVoicesVolume(float value)
    {
        PlayerPrefs.SetFloat("voicesVolume", value);
        mixer.SetFloat("VoicesVolume", LinearToDB(value));
    }

    public void OnDisable()
    {
        PlayerPrefs.Save();

        masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        voicesSlider.onValueChanged.RemoveListener(SetVoicesVolume);
    }

    private float LinearToDB(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}
