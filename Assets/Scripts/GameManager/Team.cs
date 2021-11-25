using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    private string name;
    public int score;
    public int teamID;
    public int teamSize;
    public List<Player> players = new List<Player>();

    public Team(string _name, int TeamID,int TeamSize)
    {
        name = _name;
        teamID = TeamID;
        teamSize = TeamSize;
        score = 0;

        Debug.Log(name + " Team Created! Their Team ID is " + teamID);
    }
    public bool AddPlayer(Player player) 
    {
        if (players.Count < teamSize || teamSize == -1) 
        {
            players.Add(player);
            player.team = this;
            return true;
        }
        return false;
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
        player.team = null;
    }

        public string GetTeamName()
    {
        return name;
    }
}
