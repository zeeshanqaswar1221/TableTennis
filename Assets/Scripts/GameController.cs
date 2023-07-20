using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public UIManager uiManager;
    public Ball ball;
    public GameObject player;
    public GameObject playersParent;
    
    void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    
    private void Start()
    {
        uiManager.StartCountDown(5, 5);
    }
    
    public void OnInitializeRound()
    {
        ball.gameObject.SetActive(true);
    }

   
}
