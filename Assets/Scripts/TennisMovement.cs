using System;
using UnityEngine;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;

public class TennisMovement : NetworkBehaviour, INetworkRunnerCallbacks
{
    public SpriteRenderer interpolator;

    public int paddleId;
    public bool dragging = false;
    private Vector3 offset;
    private Vector3 m_InitalPos;

    public int moveDirection = 0; // 1 means right -1 means left

    private NetworkRigidbody2D m_NetRigidbody;
    public Vector3 paddleDragDirection { get; set; }

    public float yDirectionParameter;

    private Collider2D m_Collider;

    public override void Spawned()
    {
        Runner.AddCallbacks(this);

        m_Collider = GetComponent<Collider2D>();
        m_NetRigidbody = GetComponent<NetworkRigidbody2D>();

        if (Object.HasInputAuthority)
            m_NetRigidbody.InterpolationDataSource = InterpolationDataSources.Predicted;

        paddleDragDirection = transform.forward;
        yDirectionParameter = transform.position.y / Mathf.Abs(transform.position.y);
    }

    #region New Movement

    public float minDragRadius = 1f;

    public override void FixedUpdateNetwork()
    {
        DetectCollisions();

        if (GetInput(out PaddleInput input))
        {
            if (input.IsDragging)
            {
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

                //transform.position = input.Movement;
                m_NetRigidbody.Rigidbody.MovePosition(input.Movement);
            }
        }
    }

    
    private void OnMouseDown()
    {
        if(Object != null && !Object.HasInputAuthority)
            return;
        
        moveDirection = 0;
        m_InitalPos = transform.position;
        paddleDragDirection = transform.up.normalized;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnDrawGizmos()
    {
        //if (m_InitalPos != Vector3.zero)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawRay(m_InitalPos, paddleDragDirection);
        //}
    }

    private void OnMouseUp()
    {
        if(Object!=null && !Object.HasInputAuthority)
            return;

        moveDirection = 0;
        m_InitalPos = Vector2.zero;
        dragging = false; // Stop dragging.
    }

    [Networked] public NetworkBool ballCollided { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (!Object.HasStateAuthority)
        //{
        //    return;
        //}

        //if (collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        //{
        //    ball.ReceiveCollision(paddleId, yDirectionParameter, transform.position, m_Collider.bounds.size);

        //    //if (Runner.IsServer)
        //    //{
                
        //    //}
        //    //else
        //    //{
        //    //    ball.RPC_ReceiveCollision(paddleId, yDirectionParameter, transform.position, m_Collider.bounds.size);
        //    //}
        //}
    }

    Collider2D _hitCollider;
    private void DetectCollisions()
    {
        if (!Object.HasStateAuthority)
        {
            return;
        }

        _hitCollider = Runner.GetPhysicsScene2D().OverlapBox(transform.position, m_Collider.bounds.size * 1f, 0, LayerMask.GetMask("Ball"));

        if (_hitCollider != default)
        {
            if (_hitCollider.gameObject.TryGetComponent<Ball>(out Ball ball))
            {
                ball.ReceiveCollision(paddleId, yDirectionParameter, transform.position, m_Collider.bounds.size);

                //if (Runner.IsServer)
                //{

                //}
                //else
                //{
                //    ball.RPC_ReceiveCollision(paddleId, yDirectionParameter, transform.position, m_Collider.bounds.size);
                //}
            }
        }
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
            return;

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