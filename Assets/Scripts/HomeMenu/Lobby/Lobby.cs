using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


/* TODO:
 * - Changing Lobby Title Objects causes bugs on client
 */


/*
 * The Lobby Class is used to give all LobbyPlayer an instance above them for managing purposes.
 * You could write this into a NetworkManager, but I thought it would be nicer in a seperate script.
 */

public class Lobby : NetworkBehaviour
{
    // Sync Vars
    [SyncVar] //A list of all connected player
    public List<LobbyPlayer> LobbyPlayers = new List<LobbyPlayer>(); // All player a register themselves when they join (LobbyPlayer.cs)

    [SyncVar(hook = "ChangeTitle")]
    [SerializeField] string lobbyTitle;  // Title/Name of the Lobby; Can only be changed by the host, because of "AuthHost"

    [SyncVar]
    public bool allReady = false; // All players are ready?


    void Update()
    {
        CheckLobbyPlayers(); // Checking the LobbyPlayer List
        allReady = CheckAllReady(); // Continous checking if all player are ready
    }   

    public void StartGame() // initializes the In-Game Scene and converts LobbyPlayers to GamePlayers
    {
        Debug.Log("START");
        /* https://youtu.be/HZIzGLe-2f4?t=586
         * Start Loading Panel 
         * Destroy LobbyPlayer
         * Instatiate Player Objects and connect them to "conn"
         * Switch Scene
         */
    }

    #region LobbyPlayer Interaction (Public)
    /* Public (Where the LobbyPlayer interacts with) */
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

    public void RegisterPlayer(LobbyPlayer player) // Where a Player can register himself
    {
        LobbyPlayers.Add(player);
    }
    #endregion

    #region checks
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
    #endregion

    #region hooks
    /* HOOKS */
    void ChangeTitle(string oldTitle, string newTitle) // Changes the Title Object
    {
        GameObject.Find("title").GetComponent<TextMeshProUGUI>().text = newTitle;
    }
    #endregion
}
