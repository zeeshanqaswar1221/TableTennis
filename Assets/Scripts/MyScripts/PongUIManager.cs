using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class PongUIManager : MonoBehaviour
{
    [SerializeField] Button menuBtn;
    public UiPopUp countDonwText;
    public GameObject pongGameControllerObject;
    public List<Coroutine> coroutines = new List<Coroutine>();
    public TextMeshProUGUI scoreMaster, scoreClient;
    public TwoBtnPopup redPlayerWinScreen, bluePlayerWinScreen;
    public GameObject waitingForPlayers, fadeScreen;
    public GameObject redWins, blueWins;
    public TwoBtnPopup warningPopUp;
    public RectTransform clientAvatar, masterAvatar;
    public ScoreTexts clientref, masterrerf;

    public void SetAvatarPositions(bool isClient)
    {

        if (isClient)
        {
            print("This is client");
            clientref.gameObject.SetActive(true);
            scoreMaster = clientref.scoreClient;
            scoreClient = clientref.scoreMaster;
        }
        else
        {

            masterrerf.gameObject.SetActive(true);
        }

    }

    private void Start()
    {

        // if (!PhotonNetwork.IsMasterClient)
        // {
        //     SetAvatarPositions(true);
        // }
        // else
        // {
        //     print("This is Host");

        // }


        menuBtn.onClick.AddListener(delegate { OnMenuBtnClick(); });
    }
    public void StartCountDown(int maxCount, float duration)
    {
        coroutines.Add(StartCoroutine(CountNumbers(maxCount, duration)));
        SoundManager.instance.StartCountDownSound();
    }
    public void StartFade()
    {
        /* fadeScreen.SetActive(false);
         fadeScreen.SetActive(true);*/
    }
    public void OnMenuBtnClick()
    {
        SoundManager.instance.BtnSoundPlay();

        EnableDisablePopUps(warningPopUp, PopupType.TWO_BTN,
          "Do you really want to Leave?", true,
          () =>
          {
              SoundManager.instance.BtnSoundPlay();

          },
          () =>
          {
              EnableDisablePopUps(warningPopUp, PopupType.TWO_BTN, "", false);
              SoundManager.instance.BtnSoundPlay();
          }
          );
    }
    private IEnumerator CountNumbers(int maxCount, float duration)
    {

        countDonwText.gameObject.SetActive(true);
        for (int i = maxCount; i > 0; i--)
        {
            countDonwText.infoText.text = i.ToString();
            yield return new WaitForSeconds(duration);
        }
        Debug.Log("Counting complete!");


        countDonwText.gameObject.SetActive(false);
        coroutines.Clear();

        yield return null;
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
            default:
                break;
        }


    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
