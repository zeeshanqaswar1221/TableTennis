using System;
using UnityEngine;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine.UIElements;
using DG.Tweening;

namespace Tennis.Orthographic
{
    public class TennisMovement : NetworkBehaviour, INetworkRunnerCallbacks
    {
        public SpriteRenderer interpolator;

        public LayerMask collisionLayer;
        public float collisionRadius;
        public bool dragging = false;
        private Vector3 offset;
        private Vector3 m_InitalPos;

        public Vector3 paddleDragDirection { get; set; }

        public int ForwardDir { get; set; }
        public Collider2D GetCollider2d { get; set; }

        private NetworkRigidbody2D m_NetworkRigibody;

        public override void Spawned()
        {
            Runner.AddCallbacks(this);

            m_NetworkRigibody = GetComponent<NetworkRigidbody2D>();  
            GetCollider2d = GetComponent<Collider2D>();
            ForwardDir =  (int)(transform.position.y / Mathf.Abs(transform.position.y));
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PaddleInput input))
            {
                if (input.IsDragging)
                {
                    m_NetworkRigibody.Rigidbody.MovePosition(input.Movement);
                }
            }
        }

        private void OnMouseDown()
        {
            if (!Object.HasInputAuthority)
                return;

            m_InitalPos = transform.position;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

        private void OnMouseUp()
        {
            if (!Object.HasInputAuthority)
                return;

            m_InitalPos = Vector2.zero;
            dragging = false;
        }

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
            newPos.y = ForwardDir > 0 ? Mathf.Clamp(newPos.y, 3, 1000) : Mathf.Clamp(newPos.y, -1000, -3);

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
}