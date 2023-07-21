using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion.Sockets;

public class NetworkInitializer : MonoBehaviour
{
    public GameObject networkRunnerPrefab;
    
    public NetworkRunner Runner;
    public NetworkSceneManagerBase NetworkSceneManager;

    private const int PLAYER_COUNT = 2;
    private const string LOBBY_NAME = "PongTestLobby";
    private const int GAMESCENE = 1;
    
    private void Start()
    {
        Application.targetFrameRate = 60;

        InitializeNetwork(GameMode.Shared, GAMESCENE);
    }

    private async Task<StartGameResult> InitializeNetwork(GameMode gameMode, SceneRef gameScene)
    {
        #region START RUNNER
        if (ReferenceEquals(Runner, null))
        {
            Runner = Instantiate(networkRunnerPrefab, Vector3.zero, Quaternion.identity).GetComponent<NetworkRunner>();
        }
        #endregion

        NetworkSceneManager = Runner.GetComponents(typeof(MonoBehaviour)).OfType<NetworkSceneManagerDefault>().FirstOrDefault();
        if (ReferenceEquals(NetworkSceneManager, null))
            NetworkSceneManager = Runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        


        Runner.ProvideInput = true;

        return await Runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = NetAddress.Any(),
            CustomLobbyName = LOBBY_NAME,
            PlayerCount = PLAYER_COUNT,
            SceneManager = NetworkSceneManager,
            Scene = gameScene
        });
    }
}
