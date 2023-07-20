using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject tennis;
    public GameObject playersParent;
    // Start is called before the first frame update
    void Start()
    {
        tennis = (Resources.Load<GameObject>("Players/Paddle")) as GameObject;
        if (tennis != null)
        {
            // Instantiate the loaded prefab
            //tennis = Instantiate(tennis, transform.position, Quaternion.identity);
            // (tennis.GetComponent<TennisMovement>()).OnInit(playersParent.transform, new Vector3(0, -5, 0), PlayerType.HOST);


        }
        else
        {
            Debug.LogError("Failed to load the asset!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
