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

    [SyncVar(hook = "ChangeDisplayTeam")]
    [SerializeField] int teamId = 0;

    Lobby lobby;
    public override void OnStartClient()
    {
        lobby = GameObject.Find("LobbyManager").GetComponent<Lobby>();
        lobby.RegisterPlayer(this);
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
            team1Btn = GameObject.Find("Team1Btn").GetComponent<Button>();
            team2Btn = GameObject.Find("Team2Btn").GetComponent<Button>();
            team1Btn.onClick.AddListener(delegate { SelectTeam(1); });
            team2Btn.onClick.AddListener(delegate { SelectTeam(2); });
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
    
    public void ChangeDisplayTeam(int oldTeamId, int newTeamId)
    {
        if(newTeamId == 1)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team1List").transform;
        }else if(newTeamId == 2)
        {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team2List").transform;
        }
    }
}
