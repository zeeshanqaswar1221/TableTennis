using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
public class MainMenuUiManager : MonoBehaviour
{
    public static MainMenuUiManager instance;
    [SerializeField] Button menuBtn, settingBtn;
    public Slider musicSlider, soundSlider;
    public UiPopUp musicValueText, soundValueText;
    public OneBtnPopup settingScreen;
    public GameObject LoadingPanel;


    void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
       

    }
    private void SoundPreferenceCheck()
    {
        soundSlider.value = PlayerPrefs.GetFloat("_GAMEVOLUME");
        musicSlider.value = PlayerPrefs.GetFloat("_MUSIC");
        SoundVolumeController();
        MusicVolumeController();

    }
    private void Start()
    {
        menuBtn.onClick.AddListener(() => LoadScene("Splash"));
        settingBtn.onClick.AddListener(() => OnSettingBtnClick());
        SoundPreferenceCheck();


    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void MusicVolumeController()
    {
        musicValueText.infoText.text = (musicSlider.value * 100).ToString("F0");
        PlayerPrefs.SetFloat("_MUSIC", musicSlider.value);
        SoundManager.instance.UpdateMusicVolume();
    }

    public void SoundVolumeController()
    {

        soundValueText.infoText.text = (soundSlider.value * 100).ToString("F0");
        PlayerPrefs.SetFloat("_GAMEVOLUME", (soundSlider.value));
        //print($"Actual value{soundSlider.value} with multiply{soundSlider.value * 0.01f}");
        SoundManager.instance.UpdateMasterVolume();
    }
    public void OnSettingBtnClick()
    {
        SoundManager.instance.BtnSoundPlay();

        EnableDisablePopUps(settingScreen, PopupType.ONE_BTN, "", true,
        () => EnableDisablePopUps(settingScreen, PopupType.ONE_BTN, "", false));
    }
    public void EnableDisablePopUps(UiPopUp popUp, PopupType type, string info, bool istrue, Action actionOne = null,
         Action actionTwo = null, Action actionThree = null)
    {
        if (!istrue)
        {
            popUp.gameObject.SetActive(false);
            return;
        }

        popUp.DisplayText(info);
        popUp.gameObject.SetActive(true);
        switch (type)
        {
            case PopupType.ONE_BTN:
                (popUp as OneBtnPopup)?.RegisterFunctionOnBtnOne(actionOne);
                break;
            case PopupType.TWO_BTN:
                (popUp as TwoBtnPopup)?.RegisterFunctionOnBtnOne(actionOne);
                (popUp as TwoBtnPopup)?.RegisterFunctionOnBtnTwo(actionTwo);
                break;
          /*  case PopupType.THREE_BTN:
                (popUp as ThreeBtnPopup)?.RegisterFunctionOnBtnOne(actionOne);
                (popUp as ThreeBtnPopup)?.RegisterFunctionOnBtnTwo(actionTwo);
                (popUp as ThreeBtnPopup)?.RegisterFunctionOnBtnTwo(actionThree);
                break;*/

            default:
                break;
        }
    }
}
