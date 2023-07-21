using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
/*using Photon.Pun;*/
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;

public class TennisMovement : NetworkBehaviour, INetworkRunnerCallbacks
{
    public SpriteRenderer interpolator;

    public bool dragging = false;
    private Vector3 offset;
    private Vector3 m_InitalPos;

    public int moveDirection = 0; // 1 means right -1 means left

    private Rigidbody2D m_Rigidbody;
    public Vector3 paddleDragDirection { get; set; }

    public float yDirectionParameter;

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
        
        m_Rigidbody = GetComponent<Rigidbody2D>();
        paddleDragDirection = transform.forward;
        yDirectionParameter = transform.position.y / Mathf.Abs(transform.position.y);
    }

    #region New Movement

    public float minDragRadius = 1f;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PaddleInput input))
        {
            TennisController(input.Movement,input.IsDragging);
        }
    }

    private void TennisController(Vector2 newPos,bool dragging)
    {
        if (dragging)
        {
            transform.position = newPos;
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            paddleDragDirection = transform.position - m_InitalPos;
           
            if (paddleDragDirection.magnitude > minDragRadius)
            {
                Vector3 forDirection = Vector3.Cross(Vector3.up, paddleDragDirection.normalized);
                moveDirection = forDirection.z < 0 ? 1 : forDirection.z > 0 ? -1 : 0;
            }
            else
            {
                moveDirection = 0;
            }

            // Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            // if (yDirectionParameter > 0)
            // {
            //     newPos.y = Mathf.Clamp(newPos.y, 3, 1000);
            // }
            // else
            // {
            //     newPos.y = Mathf.Clamp(newPos.y, -1000, -3);
            // }
            
            //m_Rigidbody.MovePosition(newPos);// Movement
        }
    }
    private void TennisController()
    {
        if (dragging)
        {
            
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            paddleDragDirection = transform.position - m_InitalPos;
           
            if (paddleDragDirection.magnitude > minDragRadius)
            {
                Vector3 forDirection = Vector3.Cross(Vector3.up, paddleDragDirection.normalized);
                moveDirection = forDirection.z < 0 ? 1 : forDirection.z > 0 ? -1 : 0;
            }
            else
            {
                moveDirection = 0;
            }

            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            if (yDirectionParameter > 0)
            {
                newPos.y = Mathf.Clamp(newPos.y, 3, 1000);
            }
            else
            {
                newPos.y = Mathf.Clamp(newPos.y, -1000, -3);
            }

            m_Rigidbody.MovePosition(newPos);// Movement
        }
    }
    private void OnMouseDown()
    {
        if(Object!=null&&!Object.HasInputAuthority)
            return;
        
        moveDirection = 0;
        m_InitalPos = transform.position;
        paddleDragDirection = transform.up.normalized;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnDrawGizmos()
    {

        if (m_InitalPos != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            // Gizmos.DrawLine(initalPos, transform.position);
            Gizmos.DrawRay(m_InitalPos, paddleDragDirection);
        }
    }

    private void OnMouseUp()
    {
        if(Object!=null&&!Object.HasInputAuthority)
            return;
        moveDirection = 0;
        m_InitalPos = Vector2.zero;
        dragging = false; // Stop dragging.
    }

    #endregion

    #region NETWORK CALLBACKS

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!Object.HasInputAuthority)
        {
            return;
        }
        PaddleInput paddleInput = new PaddleInput();
        paddleInput.IsDragging = dragging;
        
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        if (yDirectionParameter > 0)
        {
            newPos.y = Mathf.Clamp(newPos.y, 3, 1000);
        }
        else
        {
            newPos.y = Mathf.Clamp(newPos.y, -1000, -3);
        }

        paddleInput.Movement = newPos;
        input.Set(paddleInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    #endregion
}

public struct PaddleInput : INetworkInput
{
    public Vector2 Movement;
    public NetworkBool IsDragging;
}