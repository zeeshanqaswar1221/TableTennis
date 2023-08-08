
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThreeBtnPopup : UiPopUp
{
    [SerializeField] Button btnOne;
    [SerializeField] Button btnTwo;
    [SerializeField] Button btnThree;

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
    public void RegisterFunctionOnBtnThree(Action func)
    {
        btnThree.onClick.RemoveAllListeners();
        btnThree.onClick.AddListener(delegate { func(); });
    }

}
