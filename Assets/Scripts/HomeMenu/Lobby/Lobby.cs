using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class Lobby : NetworkBehaviour
{
    [SerializeField]
    public List<LobbyPlayer> LobbyPlayers = new List<LobbyPlayer>();

    [SyncVar(hook = "ChangeTitle")]
    [SerializeField] private string lobbyTitle;

    public bool AuthHost(LobbyPlayer player)
    {
        if(LobbyPlayers.IndexOf(player) == 0)
        {
            return true;
        }
        return false;
    }

    public void SetTitle(LobbyPlayer player, string text)
    {
        if (AuthHost(player))
        {
            lobbyTitle = text;
        }
    }

    public void RegisterPlayer(LobbyPlayer player)
    {
        LobbyPlayers.Add(player);
    }

    public void ChangeTitle(string oldTitle, string newTitle)
    {
        GameObject.Find("title").GetComponent<TextMeshProUGUI>().text = newTitle;
    }
}
