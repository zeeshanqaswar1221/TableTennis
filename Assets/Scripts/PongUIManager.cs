using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PongUIManager : MonoBehaviour
{
    public UiPopUp countDonwText;
    public GameObject pongGameControllerObject, WaitingPanel;
    public List<Coroutine> coroutines = new List<Coroutine>();
    public Text scoreMaster,scoreClient, pingText;
    public GameObject redWins,blueWins,waitingForPlayers;


    public void StartCountDown(int maxCount, float duration)
    {
        coroutines.Add(StartCoroutine(CountNumbers(maxCount, duration)));
        
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
        BeastGamesNetwork.instance.transform.GetChild(0).gameObject.SetActive(false);
        coroutines.Clear();

        yield return null;
    }
}
