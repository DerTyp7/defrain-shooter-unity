using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour
{
    [SerializeField] private Button rdyBtn;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI rdyText;

    [SerializeField] private Button team1Btn;
    [SerializeField] private Button team2Btn;

    [SyncVar(hook = "DisplayPlayerName")]
    [SerializeField] public string username;

    [SyncVar(hook = "ChangeReadyState")]
    [SerializeField] bool ready = false;

    [SyncVar]
    [SerializeField] int teamId = 0;

    Lobby lobby;
    public override void OnStartClient()
    {
        lobby = GameObject.Find("LobbyManager").GetComponent<Lobby>();
        lobby.RegisterPlayer(this);

        if(teamId == 0)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team1").transform;
        }else if(teamId == 1)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team2").transform;
        }
    }

    public void Start()
    {
        if (isLocalPlayer)
        {
            VariableSaver vs = GameObject.FindGameObjectWithTag("VariableSaver").GetComponent<VariableSaver>();
            rdyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
            rdyBtn.onClick.AddListener(CmdChangeReady);
            CmdSendName(vs.username);
            lobby.SetTitle(this, "Game Of\n" + username);
        }
    }

    [Command]
    public void CmdSendName(string playerName)
    {
        username = playerName;
    }

    [Command]
    public void CmdChangeReady()
    {
        ready = !ready;
    }

    [Command]
    public void SelectTeam(int _teamId)
    {
        teamId = _teamId;
    }
    public void DisplayPlayerName(string oldName, string newName)
    {
        Debug.Log("Player changed name from " + oldName + " to " + newName);
        usernameText.text = newName;
    }

    public void ChangeReadyState(bool oldState, bool newState)
    {
        if (newState)
        {
            rdyText.text = "Ready";
        }
        else
        {
            rdyText.text = "Not Ready";
        }
        
    }
}
