using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBounds : MonoBehaviour
{
    public Transform standingPosition;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (standingPosition != null)
        {
            if (col.gameObject.TryGetComponent(out Ball ball))
            {
                ball.Reset(standingPosition);
            }
        }
    }
}
