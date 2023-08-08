using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestForce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SceneLoadNew", 3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //collision.gameObject.GetComponent<Ball>().AddForceToBall(new Vector2(0, -30));
        }

            
    }

    public void SceneLoadNew()
    {
        SceneManager.LoadScene("AllGames");
    }
   
}
