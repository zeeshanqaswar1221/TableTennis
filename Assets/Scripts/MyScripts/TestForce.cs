using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestForce : MonoBehaviour
{
    public bool isServer;

    void Start()
    {
        if (isServer)
        {
            SceneLoadNew();
        }
        else
        {
            Invoke("SceneLoadNew", 1f);
        }
    }
   

    public void SceneLoadNew()
    {
        SceneManager.LoadScene("AllGames");
    }
   
}
