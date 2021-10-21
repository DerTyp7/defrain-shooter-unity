using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMaster : MonoBehaviour
{
    [Header("GameMaster")]
    [SerializeField] private List<Team> teams = new List<Team>();
    private void Start()
    {
        
        CreateTeam("Orange");
        CreateTeam("Blue");

        MLAPI.NetworkManager.Singleton.StartHost();
    }

    private void CreateTeam(string name, int score = 0)
    {
        Team team = new Team(name, score);
        teams.Add(team);
    }
    
    public List<Team> GetTeams()
    {
        return teams;
    } 
}
