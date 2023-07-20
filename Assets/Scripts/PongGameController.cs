using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using Photon.Pun;
using Photon.Realtime;*/
public class PongGameController : MonoBehaviour
{
    public PongUIManager UiManager;
    public Transform masterTransform, clientTransform,masterBallPosition,clientBallPosition;
    public int scoreMaster = 0,scoreClient = 0;
    public GameObject gameBall;
    
    public static PongGameController Instance;
    
    void Awake()
    {
        Instance = this;
    }

    public void UpdateScoreMaster()
    {
        scoreMaster = scoreMaster + 1;
        UiManager.scoreMaster.text = scoreMaster.ToString();
        
        if (scoreMaster >= 7)
        {
            UiManager.redWins.SetActive(true);
            Invoke("LoadMenu", 3f);
        }
    }

    public void UpdateScoreClient()
    {
        scoreClient = scoreClient + 1;
        UiManager.scoreClient.text = scoreClient.ToString();
        if (scoreClient >= 7)
        {
            UiManager.blueWins.SetActive(true);
            Invoke("LoadMenu", 3f);
        }
    }



}
