using UnityEngine;
using TMPro;

public enum PopupType
{
    ONE_BTN, TWO_BTN
}
public class UiPopUp : MonoBehaviour
{
    public TextMeshProUGUI infoText;

    public void DisplayText(string infoToDisplay)
    {
        infoText.SetText(infoToDisplay);
    }

}
