using System;
using UnityEngine;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;

namespace Tennis.Orthographic
{
    public class TennisMovement : NetworkBehaviour, INetworkRunnerCallbacks
    {
        public NetworkObject tennisGraphics;
        public TennisGraphics Graphics { get; private set; }
        public Vector3 GraphicsPos { get { return Graphics.transform.position; } }

        private bool dragging = false;
        private Vector3 offset;
        private Vector3 m_InitalPos;
        private Rigidbody2D m_Rigidbody2d;

        private Collider2D m_Collider2D;
        public float PedalWidth { get; private set; }
        [Networked] public float SwipeSpeed { get; set; }
        public int ForwardDir { get; set; }


        public override void Spawned()
        {
            Runner.AddCallbacks(this);

            m_Collider2D = GetComponent<Collider2D>();
            PedalWidth = m_Collider2D.bounds.size.x;
            m_Rigidbody2d = GetComponent<Rigidbody2D>();

            ForwardDir =  (int)(transform.position.y / Mathf.Abs(transform.position.y));
            if (Object.HasStateAuthority)
            {
                Graphics = Runner.Spawn(tennisGraphics, transform.position, tennisGraphics.transform.rotation).GetComponent<TennisGraphics>();
                Graphics.TargetToFollow = Object;
            }
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out CustomInputs input))
            {
                if (input.IsDragging)
                {
                    m_Rigidbody2d.MovePosition(input.Movement);
                }
            }
        }

        private void OnMouseDown()
        {
            if (!Object.HasInputAuthority)
                return;

            SwipeSpeed = 0f;
            m_InitalPos = transform.position;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

        private void OnMouseDrag()
        {
            SwipeSpeed = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude;
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

            CustomInputs paddleInput = new CustomInputs();
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

    public struct CustomInputs : INetworkInput
    {
        public Vector2 Movement;
        public NetworkBool IsDragging;
    }
}