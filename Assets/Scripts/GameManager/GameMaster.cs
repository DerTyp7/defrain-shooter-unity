using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// Erstellung von Teams
// Auflistung von den Spielern
// Verwaltung der Spieler und Teams
// 

public class GameMaster : NetworkBehaviour
{
    int gameState = 0;

    [Header("GameMaster")]
    [SerializeField] private List<Player> Players = new List<Player>(); 
    [SerializeField] private int countOfRounds = 10;
    public SpawnController spawnController;
    private TeamManager teamManager;


    public GameObject localPlayer;
    public void RegisterPlayer(Player player) 
    {
        Players.Add(player);
        teamManager.AddTeam().AddPlayer(player);
    }

    private void Start()
    {
        spawnController = GetComponent<SpawnController>();
        teamManager = GetComponent<TeamManager>();
    }
    private void Update()
    {
        StateMachine();

        if (Input.GetKeyDown(KeyCode.H))
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }




    void StateMachine() 
    {
        switch (gameState)
        {
            case 0:
                //Select teams
                if (Input.GetButtonDown("Sprint")) 
                {
                    gameState++;
                }
                
                break;
            case 1:
                foreach (Player player in Players)
                {
                    if (spawnController.GetAvailableSpawnPoint(player.team.teamID, out Vector3 v)) 
                    {
                        player.respawnPos(v);
                    }
                }
                gameState++;
                break;
            case 2:
                foreach (Team team in teamManager.Teams)
                {
                    if (!teamManager.teamAlive(team.teamID)) 
                    {
                        team.score++;
                        gameState++;
                    }                    
                }
                break;
            case 3:
                gameState = 0;
                Debug.Log("Team died");
                break;
            default:
                break;
        }
    }

    public Vector3 RespawnRequest(GameObject player,int teamID) 
    {
        if (spawnController.GetAvailableSpawnPoint(teamID, out Vector3 spawnpoint))
        {
            return spawnpoint;
        }
        return player.transform.position;
    }
}
