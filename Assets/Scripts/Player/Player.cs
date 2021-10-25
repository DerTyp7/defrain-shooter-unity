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

    private int health;
    private int kills;
    private int deaths;
    public override void OnStartLocalPlayer()
    {
        base.OnStartClient();
    }

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
        isAlive = true;
    }

    public void Die()
    {
        isAlive = false;
        AddDeaths(1);
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
            health -= value;
            if (health <= 0)
            {
                AddDeaths(1);
                health = 0;
                Die();
            }
        }
        
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
