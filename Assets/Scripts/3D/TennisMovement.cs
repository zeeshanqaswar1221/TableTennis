using System;
using UnityEngine;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine.UIElements;
using System.Collections;

namespace Tennis.Perspective
{
    public class TennisMovement : NetworkBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private MeshRenderer m_MeshRendere;
        [SerializeField] private Material m_MasterMaterial;
        [SerializeField] private Material m_ClientMaterial;

        private bool dragging = false;
        private Vector3 offset;
        private Vector3 m_InitalPos;


        private Transform m_Transform;
        private BoxCollider m_Collider;
        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();

        public LayerMask collisionLayer;
        public float collisionRadius = 1.25f;
        public float forwardDir;

        public Transform ResetPoint { get; set; }

        public override void Spawned()
        {
            Runner.AddCallbacks(this);
            m_Transform = transform;

            m_Collider = GetComponent<BoxCollider>();

            if (Runner.IsServer)
            {
                m_MeshRendere.sharedMaterial = Object.HasInputAuthority ? m_MasterMaterial: m_ClientMaterial;
            }
            else
            {
                m_MeshRendere.sharedMaterial = Object.HasInputAuthority ? m_ClientMaterial : m_MasterMaterial;
            }

            forwardDir = transform.position.y / Mathf.Abs(transform.position.y);
        }

        float travelDirection = 1;
        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            CheckCollision();

            if (Mathf.Abs(transform.position.x) > 4.5f)
            {
                travelDirection = travelDirection == 1 ? -1 : travelDirection == -1? 1 : 0;
            }
            
            m_Transform.Translate(Vector3.right * travelDirection * 8f * Runner.DeltaTime);

            //if (GetInput(out PaddleInput input))
            //{

            //    if (input.IsDragging)
            //    {
            //        m_NetRigidbody.Rigidbody.MovePosition(input.Movement);
            //    }
            //}
        }

        private void CheckCollision()
        {
            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapSphere(m_Transform.position, collisionRadius, Object.InputAuthority, _lagCompensatedHits, collisionLayer);
            if (count > 0)
            {
                if (_lagCompensatedHits[0].Hitbox.TryGetComponent(out Ball ball))
                {
                    if (Object.HasInputAuthority && Object.HasStateAuthority)
                    {
                        print("Master Hit The ball");
                    }
                    else
                    {
                        print("Client Hit The ball");
                    }

                    ball.SetVelocity(this);
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
            if (Object != null && !Object.HasInputAuthority)
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
            //if (forwardDir > 0)
            //{
            //    newPos.y = Mathf.Clamp(newPos.y, 3, 1000);
            //}
            //else
            //{
            //    newPos.y = Mathf.Clamp(newPos.y, -1000, -3);
            //}

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