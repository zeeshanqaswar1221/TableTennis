using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UiPopUp countDonwText;
    public List<Coroutine> coroutines = new List<Coroutine>();


    public void StartCountDown(int maxCount, float duration)
    {
        coroutines.Add(StartCoroutine(CountNumbers(maxCount, duration)));
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    private IEnumerator CountNumbers(int maxCount, float duration)
    {

        countDonwText.gameObject.SetActive(true);
        for (int i = maxCount; i >= 0; i--)
        {
            countDonwText.infoText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Counting complete!");


        countDonwText.gameObject.SetActive(false);
        GameController.instance.OnInitializeRound();
        coroutines.Clear();

        yield return null;


    }
}
