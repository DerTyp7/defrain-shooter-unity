using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeamManager : NetworkBehaviour
{
    [SerializeField]
    public List<Team> Teams = new List<Team>();
    int teamIdCount = 0;

    public void Start()
    {
        //Create Speactator Team with index 0
        Team specTeam = new Team("Spectators", 0, -1);
       
        Teams.Add(specTeam);

    }

    public Team AddTeam(string name = "Team") 
    {
        Team team = new Team(name, teamIdCount, -1);
        Teams.Add(team);
        teamIdCount++;
        return team;
    }
    public bool teamAlive(int teamID) 
    {
        bool tAlive = false;
        foreach (Player player in Teams[teamID].players)
        {
            if (player.isAlive) tAlive = true;
        }
        if (Teams[teamID].players.Count == 0) tAlive = true;
        return tAlive;
    }
    public Team AddPlayerToRandomTeam(Player player) 
    {
        int trys = 0;
        while(trys < 10)
        {
            int index = (int)Random.Range(0, Teams.Count - 1);
            if (Teams[index].AddPlayer(player)) return Teams[index];
            trys++;
        }
        return null;
    }

    public Team GetTeamByID(int TeamID)
    {
        for (int i = 0; i < Teams.Count; i ++)
        {
            if (Teams[i].teamID == TeamID) return Teams[i];
        }
        return null;
    }

}
