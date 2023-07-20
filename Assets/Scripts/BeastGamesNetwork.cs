using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using Photon.Pun;
using Photon.Realtime;*/
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeastGamesNetwork : MonoBehaviour
{
    public string[] gameplaySceneNames; // Names of the gameplay scenes for each mini-game
    private int currentGameIndex = 0; // Index of the current mini-game
    public Text infoText; // Reference to the UI text for displaying information
    public static BeastGamesNetwork instance;
   // private PhotonView photonView;
    private bool gameEnded = false;
    public bool testing,masterdone;
    public Text pingText;

    private void Update()
    {
      //  pingText.text = PhotonNetwork.GetPing().ToString();
    }
    public enum GamesNames
    {
        Pong, Game2, Game3
    }
    GamesNames currentGame;
    int gameIndex, totalRooms = 0;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
     //   ConnectToPhoton(); 
    }
    private void Start()
    {
       // photonView = GetComponent<PhotonView>();
    }

  
/*    private void ConnectToPhoton()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!PhotonNetwork.IsConnected)
            {
                ServerSettings.ResetBestRegionCodeInPreferences();
                PhotonNetwork.NetworkingClient.SerializationProtocol = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV18;
                PhotonNetwork.OfflineMode = false;
                PhotonNetwork.ConnectUsingSettings();             
            }
            else
            {
                infoText.text ="Retry: Server Connection Failed";
            }
        }
        else
        {
            infoText.text = "Please Check your Internet Connection";
        }
        
    }*/
/*    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed To Join ");


    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        infoText.text = ("Connected to server");
        //  PongUIManager.UiInstance.WaitingPanel.SetActive(true);
        //JoinMiniGameRoom(0);
        PhotonNetwork.LoadLevel("AllGames");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Random Room Joining Failed, Creating New Room");
        CreateRoom();
    }*/
    public void JoinGame()
    {
       // PhotonNetwork.JoinRandomRoom();
    }
    public void JoinMiniGameRoom(int gameIndex)
    {
       /* currentGame = GamesNames.Pong;
        totalRooms = PhotonNetwork.CountOfRooms;
        string RoomName = currentGame.ToString() + totalRooms;

        if (PhotonNetwork.CountOfRooms == 0 || PhotonNetwork.CountOfRooms < totalRooms)
        {
            RoomOptions currentOption = new RoomOptions();
            currentOption.MaxPlayers = 2;
            currentOption.IsOpen = true;
            currentOption.IsVisible = true;
            PhotonNetwork.CreateRoom(RoomName, currentOption, null);

        }
        else
        {
            PhotonNetwork.JoinRoom(RoomName);
           
        }*/


    }
    public void CreateRoom()
    {
       /* currentGame = GamesNames.Pong;
        totalRooms = PhotonNetwork.CountOfRooms + 1;
        string RoomName = currentGame.ToString() + totalRooms;
        RoomOptions currentOption = new RoomOptions();
        currentOption.MaxPlayers = 2;
        currentOption.IsOpen = true;
        currentOption.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(RoomName, currentOption, null);*/
    }
    /*public override void OnCreatedRoom()
    {
        Debug.Log("Room created. Waiting for players...");
        infoText.text = ("Room created.");
       // this.transform.GetChild(0).gameObject.SetActive(false);
    }*/
   // [PunRPC]
    public void DisableWaiting()
    {   
      //  totalRooms = PhotonNetwork.CountOfRooms + 1;
        
    }



/*
    public override void OnJoinedRoom()
    {
        PongGameController.instance.UiManager.waitingForPlayers.SetActive(true);
        infoText.text = ("Room Joined.");
        Debug.Log("waiting");
        if (testing)
        {
            PongGameController.instance.UiManager.waitingForPlayers.SetActive(false);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                //photonView.RPC("DisableWaiting", RpcTarget.All);
                photonView.RPC("SpawnPlayers", RpcTarget.All);
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                //photonView.RPC("DisableWaiting", RpcTarget.All);
                photonView.RPC("SpawnPlayers",RpcTarget.All);
            }
        }

    }
*/
  

// [PunRPC]
    void SpawnPlayers()
    {
        /*if (PhotonNetwork.IsMasterClient)
        {
            PongGameController.instance.masterPlayer = PhotonNetwork.Instantiate("MasterPaddle", PongGameController.instance.masterTransform.position, Quaternion.identity);
            StartCoroutine(StartCountDown());
        }
        else
        {
            PongGameController.instance.cameraController.Vr1.transform.rotation = Quaternion.Euler(0, 0, -180);
            PongGameController.instance.clientPlayer = PhotonNetwork.Instantiate("ClientPaddle", PongGameController.instance.clientTransform.position, PongGameController.instance.clientTransform.rotation);
            StartCoroutine(StartCountDown());
        }*/
        
    }

   // [PunRPC]
    void MasterComplete()
    {
        masterdone = true;
    }

   // [PunRPC]
    IEnumerator StartCountDown()
    {
        /* if (PhotonNetwork.IsMasterClient)
         {
             yield return new WaitUntil(() => PongGameController.instance.startCounting);
             PongGameController.instance.UiManager.waitingForPlayers.SetActive(false);
             PongGameController.instance.UiManager.StartCountDown(3, 1.5f);
             photonView.RPC("MasterComplete", RpcTarget.All);
         }
         else
         {
             yield return new WaitUntil(() => PongGameController.instance.startCounting && masterdone);
             PongGameController.instance.UiManager.waitingForPlayers.SetActive(false);
             PongGameController.instance.UiManager.StartCountDown(3, 1.5f);
         }*/
        yield return new WaitForSeconds(0);
    }
   
  

    private void LoadMenu()
    {
       // PhotonNetwork.LoadLevel("AllGames");
        //this.transform.GetChild(0).gameObject.SetActive(true);
    }

   /* public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!gameEnded)
        {
            if (otherPlayer.IsMasterClient)
            {
                PongGameController.instance.UiManager.blueWins.SetActive(true);
                PhotonNetwork.LeaveRoom();
                Invoke("LoadMenu", 3f);
            }
            else
            {
                PongGameController.instance.UiManager.redWins.SetActive(true);
                PhotonNetwork.LeaveRoom();
                Invoke("LoadMenu", 3f);
            }
            EndGame("Opponent left the game. You win!");
            infoText.text = ("Opponent left the game. You win!");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (!gameEnded)
        {
            EndGame("Disconnected from server.");
            infoText.text = ("Disconnected from server.");
        }
    }

    private void EndGame(string message)
    {
        gameEnded = true;

        // Perform any necessary game-ending actions, such as disabling player input or showing game over UI

        UpdateInfoText(message);

        // Leave the room and disconnect from Photon
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }*/

    private void UpdateInfoText(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }
    }
}
