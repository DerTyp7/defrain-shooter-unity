using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public bool isAlive;
    public Team team;
    [SerializeField] private const int defaultHp = 100;


    public ulong clientId;

    [SyncVar(hook = nameof(SetName))]
    public string username;

    [SerializeField] GameObject usernameTextObj;


    public override void OnStartLocalPlayer()
    {
        base.OnStartClient();

        //Load Player Username;

    }

    public void SetName(string oldName, string newName)
    {
        username = newName;
        usernameTextObj.GetComponent<TMPro.TextMeshPro>().SetText(username);
    }
}
