using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour
{
    [SerializeField] private Button rdyBtn;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI rdyText;

    [SyncVar(hook = "DisplayPlayerName")]
    [SerializeField] public string username;

    [SyncVar(hook = "ChangeReadyState")]
    [SerializeField] bool ready = false;

    Lobby lobby;
    public override void OnStartClient()
    {
        lobby = GameObject.Find("LobbyManager").GetComponent<Lobby>();
        lobby.RegisterPlayer(this);

        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Team1").transform;
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
