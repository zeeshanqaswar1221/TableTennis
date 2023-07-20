using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;

[RequireComponent(typeof(NetworkRunner))]


public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public static BasicSpawner _instance;

    public static BasicSpawner basicSpawner;
    public NetworkRunner _runner;
  //  public NetworkPlayer playerPrefab;
    private Vector3 direction;
    /*[SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();*/

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _runner = GetComponent<NetworkRunner>();

    }
    private void Start()
    {
        InitializeNetworkRunner(_runner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }
    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialize)
    {
        runner.ProvideInput = true;
        Debug.Log("Session Initializing");

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "SessionName",
            Initialized = initialize,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected To Server");
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) 
    {
        Debug.Log("Input Missing");
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) 
    {
        Debug.Log("Server Shutdown");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected From Server");
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) 
    {
        Debug.Log("Trying To Connect");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("Connection Failed");
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if(runner.IsServer)
        {
            Vector3 spawnLocation = Vector3.zero;
            Debug.Log("Player joined");
         //   runner.Spawn(playerPrefab, spawnLocation, Quaternion.identity, player.PlayerId);
            //runner.Spawn(playerPrefab, spawnLocation, Quaternion.identity, player.PlayerId);

        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {

    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        /*if(NetworkPlayer.Local != null)
        {
           // PlayerInputHandler localPlayerInputHandler = NetworkPlayer.Local.GetComponent<PlayerInputHandler>();
          *//*  if(localPlayerInputHandler != null)
            {
                //input.Set(localPlayerInputHandler.GetNetworkInput());
            }*//*
        }*/
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

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
