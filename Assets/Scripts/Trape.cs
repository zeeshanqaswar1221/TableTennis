using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trape : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Ball") return;
        Ball ball = other.gameObject.GetComponent<Ball>();
        ball.isHit = false;
        ball.Bounce();

        //        print("trigger");

    }

}
