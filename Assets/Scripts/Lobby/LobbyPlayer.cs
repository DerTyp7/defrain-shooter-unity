using UnityEngine;
using UnityEngine.UI; // For <Button>
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;

/* 
 * This class manages the LobbyPlayer, which is the player object while the player is connected only to the lobby
 * AND is not IN-GAME (just a text-based player)!   
 * this LobbyPlayer will get converted into a Player/GamePlayer for in-game use.
 */

public class LobbyPlayer : NetworkBehaviour
{
    // UI Objects
    [SerializeField] Button rdyBtn; // Button Object for listeners
    [SerializeField] TextMeshProUGUI rdyButtonText; // Seperatly getting the text, because it's safer instead of "rdy.Btn.getChild..."

    [SerializeField] TextMeshProUGUI usernameText; // Username Text Object -> Where the Username of every LobbyPlayer will get displayed
    [SerializeField] TextMeshProUGUI rdyText; // Ready Text Object -> Where the Ready-State of every LobbyPlayer will get displayed

    [SerializeField] Button team1Btn; // Join Team 1 Button
    [SerializeField] Button team2Btn; // Join Team 2 Button

    // Sync vars
    [SyncVar(hook = "DisplayPlayerName")]
    [SerializeField] public string username;

    [SyncVar(hook = "ChangeReadyState")]
    public bool ready = false; // is the LobbyPlayer ready?

    [SyncVar(hook = "ChangeDisplayTeam")]
    [SerializeField] int teamId = 0; // which team did the player choose?

    // Vars
    Lobby lobby;

    public override void OnStartClient()
    {
        lobby = GameObject.Find("LobbyManager").GetComponent<Lobby>(); // Get the Lobby Object in Scene
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            lobby.RegisterLobbyPlayer(this); // Register the LobbyPlayer, so the lobby can store him in a list for future use
        }
        else
        {
            lobby.ChangeToPlayer(this);            
        }
        
    }

    public void Start()
    {
        if (isLocalPlayer && SceneManager.GetActiveScene().name == "Lobby") // Needs to check Scene for itself -> it starts faster than the lobby
        {
            /*
             * The Varaible Saver is used to store vars across different scenes and Player-Objects(LobbyPlayer/GamePlayer).
             * In this case it's used to get the values of the players BEFORE they joined the server.
             * -> username input field
             */
            VariableSaver vs = GameObject.FindGameObjectWithTag("VariableSaver").GetComponent<VariableSaver>(); 

            // Find GameObjects in Scene BY NAME
            rdyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
            rdyButtonText = GameObject.Find("RdyBtnText").GetComponent<TextMeshProUGUI>();
            team1Btn = GameObject.Find("Team1Btn").GetComponent<Button>();
            team2Btn = GameObject.Find("Team2Btn").GetComponent<Button>();
            
            // Set Button Listeners
            rdyBtn.onClick.AddListener(CmdChangeReady);
            team1Btn.onClick.AddListener(delegate { SelectTeam(1); });
            team2Btn.onClick.AddListener(delegate { SelectTeam(2); });

            // Send the username from the local variable saver to the synced var
            CmdSendName(vs.username);

            // Set the lobby title -> only works if you're the host
            lobby.SetTitle(this, "Game Of\n" + username);
        }
    }

    void Update()
    {
        if (isLocalPlayer && lobby.isLobbyScene)
        {
            /*
             * Update the "Ready-Button":
             * Change the text based on host or client / ready or not ready.
             * Adds and removes Start Listener to the hosts button
             */

            rdyBtn.onClick.RemoveListener(CmdStartGame); // Clear listener
            if (lobby.AuthHost(this) && lobby.allReady) // If all players are ready and your the host
            {
                rdyBtn.onClick.AddListener(CmdStartGame);
                rdyButtonText.SetText("Start");
            }
            else // You are not the host OR not all Players are ready
            {
                if (ready) // You are already ready
                {
                    rdyButtonText.SetText("Un-Ready");
                }
                else // You  are not ready
                {
                    rdyButtonText.SetText("Ready");
                }
            }
        }
    }

    #region hooks
    /* HOOKS */
    public void DisplayPlayerName(string oldName, string newName) // Changes the text value of the Player-Username-GameObject
    {
        Debug.Log("Player changed name from " + oldName + " to " + newName); // Just for debug -> No future use
        usernameText.text = newName; // sets the new text in the gameobject
    }

    public void ChangeReadyState(bool oldState, bool newState) // Changes the Ready-Text-Object of the player
    {
        // sets the new text based on the ready state of the player
        if (newState)
        {
            rdyText.text = "Ready";
        }
        else
        {
            rdyText.text = "Not Ready";
        }
    }

    public void ChangeDisplayTeam(int oldTeamId, int newTeamId) // moves the player into the correct team-list
    {
        // moves the player based on which team he has choosen
        if (newTeamId == 1)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team1List").transform;
        }
        else if (newTeamId == 2)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team2List").transform;
        }
    }
    #endregion

    #region commands
    /* COMMANDS */
    [Command]
    void CmdStartGame()
    {
        if (lobby.AuthHost(this))
        {
            lobby.StartGame();
        }
    }

    [Command]
    void CmdSendName(string playerName) //Send/Set the username from local to the synced var
    {
        username = playerName;
    }

    [Command]
    void CmdChangeReady() // Updates the Ready-State of the Player (Synced Var)
    {
        ready = !ready;
    }

    [Command]
    void SelectTeam(int _teamId) // Updates the team of the Player (Synced Var)
    {
        teamId = _teamId;
    }
    #endregion
}
