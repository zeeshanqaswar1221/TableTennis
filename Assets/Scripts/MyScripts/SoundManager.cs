using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource btnAudio;
    public AudioSource musicAudio;
    public AudioSource countDownSound;
    public AudioClip countDownAudioClip;

    private void Awake()
    {
        Debug.Log("This is SoundManager");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("_MUSIC") || !PlayerPrefs.HasKey("_GAMEVOLUME"))
        {
            Debug.Log("Sound Default Values Implemented");
            Debug.Log("Player has no music key value has been assigned");
            PlayerPrefs.SetFloat("_GAMEVOLUME", 0.5f);
            PlayerPrefs.SetFloat("_MUSIC", 0.2f);
        }

        UpdateMasterVolume();
        UpdateMusicVolume();
    }

    public void BtnSoundPlay()
    {
        btnAudio.Play();
    }

    public void UpdateMusicVolume()
    {
        musicAudio.volume = PlayerPrefs.GetFloat("_MUSIC");
    }

    public void UpdateMasterVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("_GAMEVOLUME");
    }

    public void StartCountDownSound()
    {
        countDownSound.clip = countDownAudioClip;
        countDownSound.Play();
    }

    public void PauseVoiceOver()
    {
        countDownSound.Pause();
    }

    public void ResumeVoiceOver()
    {
        countDownSound.UnPause();
    }

    public void StopVoiceOver()
    {
        countDownSound.Stop();
    }

    public void MuteAllSound()
    {
        countDownSound.Stop();
        musicAudio.Stop();
    }

    public void MusicVolumeController()
    {
        MainMenuUiManager.instance.musicValueText.infoText.text = MainMenuUiManager.instance.musicSlider.value.ToString();
        PlayerPrefs.SetFloat("_MUSIC", MainMenuUiManager.instance.musicSlider.value);
        UpdateMusicVolume();
    }

    public void SoundVolumeController()
    {
        MainMenuUiManager.instance.soundValueText.infoText.text = MainMenuUiManager.instance.soundSlider.value.ToString();
        PlayerPrefs.SetFloat("_GAMEVOLUME", MainMenuUiManager.instance.soundSlider.value);
        UpdateMasterVolume();
    }
}
