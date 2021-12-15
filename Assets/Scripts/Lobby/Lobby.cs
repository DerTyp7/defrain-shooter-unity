using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;


/* TODO:
 * - Changing Lobby Title Objects causes bugs on client
 */


/*
 * The Lobby will always be the "Room", where all Players are connected to.
 * In-Game AND in LobbyScene!
 * It manages the Players
 */

public class Lobby : NetworkBehaviour
{
    NetManagerScript networkManager;
    public bool isLobbyScene;

    //Player Lists
    [SyncVar]
    public List<Player> Players = new List<Player>();
    [SyncVar]
    public List<LobbyPlayer> LobbyPlayers = new List<LobbyPlayer>(); // All player a register themselves when they join (LobbyPlayer.cs)

    //Scene switch to in-game
    [SerializeField] GameObject GamePlayerPrefab;
    [SerializeField] [Scene] string gameScene;

    //Lobby Scene
    [SyncVar(hook = "ChangeTitle")]
    [SerializeField] string lobbyTitle;  // Title/Name of the Lobby; Can only be changed by the host, because of "AuthHost"

    [SyncVar]
    public bool allReady = false; // All players are ready?

    void Start()
    {
        DontDestroyOnLoad(this);
        networkManager = GameObject.Find("NetManager").GetComponent<NetManagerScript>();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Lobby") // Check if we are in-game
            isLobbyScene = true;
        else
            isLobbyScene = false;

        if (isLobbyScene)
        {
            CheckLobbyPlayers(); // Checking the LobbyPlayer List
            allReady = CheckAllReady(); // Continous checking if all player are ready
        }
        else
        {
            CheckPlayers();// Checking the Player List
        }
    }

 
    public void ChangeToPlayer(LobbyPlayer lobbyPlayer) //Convert/Change the LobbyPlayer to a Player
    {
        Debug.Log("Change");
        var conn = lobbyPlayer.connectionToClient;
        var newPlayerInstance = Instantiate(GamePlayerPrefab);

        //newPlayerInstance.GetComponent<Player>().username = player.username;

        NetworkServer.Destroy(conn.identity.gameObject);

        NetworkServer.ReplacePlayerForConnection(conn, newPlayerInstance.gameObject);
        LobbyPlayers.Remove(lobbyPlayer);
        Players.Add(newPlayerInstance.gameObject.GetComponent<Player>());
    }

    

    void CheckPlayers()
    {
        foreach (Player player in Players)
        {
            if (player == null)
            {
                Players.Remove(player);
            }
        }
    }

    #region InLobbyScene
    public void StartGame() // initializes the In-Game Scene
    {
        networkManager.ServerChangeScene(gameScene);
    }

    public bool AuthHost(LobbyPlayer player) // Checks if player is the host
    {
        // In theory the host should always be the first connected player, which means he is index 0 in the LobbyPlayers-List
        if (LobbyPlayers.IndexOf(player) == 0)
        {
            return true;
        }
        return false;
    }

    public void SetTitle(LobbyPlayer player, string text) // the host can set the LobbyTitle
    {
        if (AuthHost(player))
        {
            lobbyTitle = text;
        }
    }

    public void RegisterLobbyPlayer(LobbyPlayer player) // Where a LobbyPlayer can register himself
    {
        LobbyPlayers.Add(player);
    }
    public void UnregisterLobbyPlayer(LobbyPlayer player) // Where a LobbyPlayer can unregister himself
    {
        LobbyPlayers.Remove(player);
    }

    /* Checks */
    bool CheckAllReady() // Checks if all players are ready
    {
        // Check if all players are ready (if a player is not ready)
        foreach (LobbyPlayer player in LobbyPlayers)
        {
            if (!player.ready)
            {
                return false;
            }
        }
        return true;
    }

    void CheckLobbyPlayers() // Checks if all LobbyPlayers in the list are still connected (having a GameObject) -> Clears missing players
    {
        foreach(LobbyPlayer player in LobbyPlayers)
        {
            if (player == null)
            {
                LobbyPlayers.Remove(player);
            }
        }
    }

    /* HOOKS */
    void ChangeTitle(string oldTitle, string newTitle) // Changes the Title Object
    {
        GameObject.Find("title").GetComponent<TextMeshProUGUI>().text = newTitle;
    }
    #endregion
}
