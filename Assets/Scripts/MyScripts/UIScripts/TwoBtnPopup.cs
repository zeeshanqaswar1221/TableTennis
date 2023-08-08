using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoBtnPopup : UiPopUp
{
    [SerializeField] Button btnOne;
    [SerializeField] Button btnTwo;

    public void RegisterFunctionOnBtnOne(Action func)
    {
        btnOne.onClick.RemoveAllListeners();
        btnOne.onClick.AddListener(delegate { func(); });
    }
    public void RegisterFunctionOnBtnTwo(Action func)
    {
        btnTwo.onClick.RemoveAllListeners();
        btnTwo.onClick.AddListener(delegate { func(); });
    }

}
