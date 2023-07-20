using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class ScoreZone : MonoBehaviour
{
    public bool isMaster;
    bool scoreCall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag != "Ball") return;
        if(scoreCall == false)
        {  
          //  photonView.RPC("UpdateScoreRPC", RpcTarget.All);
            scoreCall = true;
        }
        StartCoroutine(UpdateScoreCall());
        
    }
   private IEnumerator  UpdateScoreCall()
    {
        yield return new WaitForSeconds(1f);
        scoreCall = false;
    }

   // [PunRPC]
    private void UpdateScoreRPC()
    {
        if (isMaster)
        {
            PongGameController.Instance.UpdateScoreClient();
            PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PongGameController.Instance.gameBall.transform.position = PongGameController.Instance.masterBallPosition.position;
        }
        else
        {
            PongGameController.Instance.UpdateScoreMaster();
            PongGameController.Instance.gameBall.GetComponent<Rigidbody2D >().velocity = Vector2.zero;
            PongGameController.Instance.gameBall.transform.position = PongGameController.Instance.clientBallPosition.position;
        }
    }



}
