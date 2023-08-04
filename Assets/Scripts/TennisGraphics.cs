using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisGraphics : NetworkBehaviour
{
    public float moveSpeed = 8;
    [Networked]public NetworkObject TargetToFollow { get; set; }

    private void Update()
    {
        if (TargetToFollow == null)
            return;
        
        transform.position = Vector3.Lerp(transform.position, TargetToFollow.transform.position, moveSpeed * Time.deltaTime);
    }
}
