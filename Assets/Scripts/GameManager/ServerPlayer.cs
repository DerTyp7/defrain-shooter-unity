using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer
{
    public GameObject Player;
    public bool isAlive;
    public Team team;
    private int health;
    private int kills;
    private int deaths;
    private const int defaultHp = 100;

    public ServerPlayer(GameObject _Player, bool _isAlive = true, int _health = defaultHp, int _kills = 0, int _deaths = 0, Team _team = null)
    {
        Player = _Player;
        isAlive = _isAlive;
        health = _health;
        kills = _kills;
        deaths = _deaths;
        team = _team;
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
