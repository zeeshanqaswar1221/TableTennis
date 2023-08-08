using UnityEngine;
using UnityEngine.UI;
using System;

public class OneBtnPopup : UiPopUp
{
    [SerializeField] Button btnOne;

    public void RegisterFunctionOnBtnOne(Action func)
    {
        btnOne.onClick.RemoveAllListeners();
        btnOne.onClick.AddListener(delegate { func(); });
    }

}
