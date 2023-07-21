
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class FusionManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public GameObject waitingScreen;
    
    private NetworkObject currentObject;
    public GameObject masterPrefab, clientPrefab,ballPrefab;

    public static FusionManager Instance;

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
        
        Instance = this;

        if (Runner.IsSharedModeMasterClient)
            waitingScreen.SetActive(true);

        if (Runner.IsSharedModeMasterClient)
        {
            Runner.Spawn(masterPrefab, PongGameController.Instance.masterTransform.position, Quaternion.identity, Runner.LocalPlayer);
            NetworkObject ball = Runner.Spawn(PongGameController.Instance.gameBall, PongGameController.Instance.masterBallPosition.position, Quaternion.identity);
            PongGameController.Instance.gameBall = ball.gameObject;
        }

    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() == 2)
            waitingScreen.SetActive(false);

        if (runner.IsSharedModeMasterClient)
            return;

        runner.Spawn(clientPrefab, PongGameController.Instance.clientTransform.position, clientPrefab.transform.rotation, player);

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
