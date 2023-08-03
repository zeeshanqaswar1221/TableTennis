
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Tennis.Orthographic;
using FusionPong.Game;

public class FusionManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public bool clientDebugMode;
    public static event Action<NetworkRunner, NetworkInput> OnInputCallBack;
    public GameObject waitingScreen;

    public Transform masterPosition, clientPosition, masterBallPosition, clientBallPosition;
    public GameObject clientPedalPrefab, MasterPedalPrefab ,ballPrefab;

    public TennisMovement MasterPedal { get; set; }
    public TennisMovement ClientPedal { get; set; }

    public static FusionManager Instance;

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
        Instance = this;

        if (Runner.IsServer)
            waitingScreen.SetActive(true);

    }

    BallController ball;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!Runner.IsServer)
            return;

        if (runner.ActivePlayers.Count() == 2)
            waitingScreen.SetActive(false);

        if (player == Runner.LocalPlayer)
        {
            if (clientDebugMode)
            {
                Runner.Spawn(ballPrefab, clientBallPosition.position, Quaternion.identity);
                return;
            }

            ball = Runner.Spawn(ballPrefab).GetComponent<BallController>();
            MasterPedal = Runner.Spawn(MasterPedalPrefab, masterPosition.localPosition, Quaternion.identity, Runner.LocalPlayer).GetComponent<TennisMovement>();
        }
        else
        {
            ClientPedal = runner.Spawn(clientPedalPrefab, clientPosition.localPosition, clientPedalPrefab.transform.rotation, player).GetComponent<TennisMovement>();
            StartCoroutine(ball.StartBall(1f));
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
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
}
