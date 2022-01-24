using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct Item
{
    public string name;
    public int ammo;
}

public class Player : NetworkBehaviour
{
    Lobby lobby;
    public bool isAlive = true;
    public Team team;

    [SerializeField] PlayerUIController playerUIController;
    [SerializeField] private const int defaultHp = 100;
    GameObject GameManager;
    GameMaster gameMaster;

    public ulong clientId;

    [SyncVar(hook = nameof(SetName))] 
    public string username;

    [SerializeField] GameObject usernameTextObj;

    [SerializeField] [SyncVar] public int health = 100;
    private int kills;
    private int deaths;

    public readonly SyncList<Item> inventory = new SyncList<Item>();

    private void Start()
    {
        lobby = GameObject.Find("LobbyManager").GetComponent<Lobby>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                playerUIController.showHit();
                
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                CmdAddWeaponItem(new Item());
            }
        }
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartClient();
        
    }


    #region INVENTORY
    
    [Command]
    public void CmdAddWeaponItem(Item item)
    {
        inventory.Add(item);
    }

    #endregion





    public void SetName(string oldName, string newName)
    {
        username = newName;
        usernameTextObj.GetComponent<TMPro.TextMeshPro>().SetText(username);
    }

    public string GetName()
    {
        return name;
    }
    public void Respawn()
    {
        
        
    }
    [Command]
    void CmdRespawnRequest() 
    {
        respawnPos(gameMaster.RespawnRequest(this.gameObject, team.teamID));
        isAlive = true;
    }

    [ClientRpc]
    public void respawnPos(Vector3 pos) 
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = pos;
        GetComponent<CharacterController>().enabled = true;
        health = defaultHp;
        isAlive = true;
    }

    public void Die()
    {
        isAlive = false;
        AddDeaths(1);
        Debug.Log("DIE");
    }

    //Health
    public void AddHealth(int value)
    {
        if (isAlive)
        {
            health += value;
        }
        
    }
    public void RemoveHealth(int value)
    {
        if (isAlive)
        {
            ShowHit();
            health -= value;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

    [ClientRpc]
    private void ShowHit() 
    {
        playerUIController.showHit();
    }
    public void SetHealth(int value)
    {
        if (isAlive)
        {
            health = value;
            if (health <= 0)
            {
                AddDeaths(1);
                health = 0;
                Die();
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    //Kills
    public void AddKills(int value)
    {
        kills += value;
    }
    public void RemoveKills(int value)
    {
        kills -= value;
    }
    public void SetKills(int value)
    {
        kills = value;
    }

    public int GetKills()
    {
        return kills;
    }

    //Deaths
    public void AddDeaths(int value)
    {
        deaths += value;
    }
    public void RemoveDeaths(int value)
    {
        deaths -= value;
    }
    public void SetDeaths(int value)
    {
        deaths = value;
    }

    public int GetDeaths()
    {
        return deaths;
    }

}




