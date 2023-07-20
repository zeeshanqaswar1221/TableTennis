using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> gameObjectsToEnable;
    public float activeTime;
    private void OnEnable()
    {

        foreach (var item in gameObjectsToEnable)
        {
            item.SetActive(GetRandomBoolean());

        }

        Invoke("EnableObjectWithDelay", activeTime);
    }
    private void EnableObjectWithDelay()
    {
        this.gameObject.SetActive(false);
    }
    public bool GetRandomBoolean()
    {
        return Random.Range(0, 2) == 0;
    }
}
