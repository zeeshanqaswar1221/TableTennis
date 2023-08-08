using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongRoomLoader : MonoBehaviour
{
    public static PongRoomLoader instance;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
      /*  if (BeastGamesNetwork.instance.gameRestart)
        {
            gameRoom.SetActive(false);
            Invoke("PingPongScene", 2f);
            BeastGamesNetwork.instance.gameRestart = false;
        }*/
    }
    public void PingPongScene()
    {
        SoundManager.instance.BtnSoundPlay();
        SceneManager.LoadScene("PingPong");
    }


}
