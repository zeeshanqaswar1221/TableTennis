using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tennis.Orthographic
{
    public class GameBounds : NetworkBehaviour
    {
        public Transform standingPosition;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (standingPosition != null)
            {
                if (col.gameObject.TryGetComponent(out IReset ball))
                {
                    ball.Reset(standingPosition.position);
                }
            }
        }
    }
}
