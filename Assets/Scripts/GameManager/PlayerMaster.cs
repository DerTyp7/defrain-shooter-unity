using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
JOIN
1. Wenn ein Spieler joined wird sichergestellt, dass er nicht in der Liste ist (fürs error handling).
2. Dann wird er in die Liste "Players" eingetragen.
3. Player bekommt einen Eintrag in die Health-Liste (Der Schlüssel ist hier, dass der Index bei beiden Listen für den gleichen Spieler stehen)

LEAVE
1. Index von dem Spieler, in der Liste, wird gesucht
2. Spieler wird aus der Liste entfernt
3. Player wird aus der Health liste ausgetragen


HEALTH
 - Set Health of a player
 - Subtract Health from a player
 - Add Health to a player

DEAD
 - Player Dies
 - Another Player gets a kill

*/
public class PlayerMaster : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players = new List<GameObject>(); //Contains All Players which are currently connected/in-game
    [SerializeField] private List<int> Health = new List<int>();
    [SerializeField] private List<int> Kills = new List<int>();
    [SerializeField] private List<int> Deaths = new List<int>();
    [SerializeField] private int defaultHp = 100;

    //JUST FOR DEBUG
    [SerializeField] private GameObject TestPlayer;

    private void Update()
    {
        //JUST FOR DEBUG
        /*Debug.Log(GetHealthOfPlayer(TestPlayer));
        SubstractHealthFromPlayer(TestPlayer, 1);*/
    }


    private void Start()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player")); //Add All Player-GameObjects into a List

        //Init Health List
        foreach(GameObject player in Players)
        {
            Health.Add(defaultHp);
            Kills.Add(0);
            Deaths.Add(0);
        }

    }

    //Join
    public void OnPlayerJoin(GameObject player) //When a Player joins
    {
        Debug.Log("Player joined"); //Give Console Feedback
        if (!Players.Contains(player)) //If the Player is NOT in the "Players-List" (For Error Handling)
        {
            Players.Add(player); //Add New Player To List
            Health.Add(defaultHp); //Add New Health to the END of the list
            Kills.Add(0);
            Deaths.Add(0);
            Debug.Log("Player added to list"); //Feedback
        }
        else
        {
            Debug.LogError("Player already exits in list"); //Error, because the "new" Player is already in the list -> !critical Anomaly!
        }
    }

    //Leave
    public void OnPlayerLeave(GameObject player) //When a Player leaves
    {
        Debug.Log("Player left");//Give Console Feedback
        if (Players.Contains(player))//If the Player IS in the "Players-List" (For Error Handling)
        {
            Players.Remove(player); //Remove the Player from List
            Health.Remove(Players.IndexOf(player)); //Remove the specific Health of the Player
            Kills.Remove(Players.IndexOf(player));
            Deaths.Remove(Players.IndexOf(player));
        }
        else
        {
            Debug.LogError("Player not found in Players-list"); //Error, because the Player is NOT in the list -> !critical Anomaly!
        }
    }

    //Health
    private void CheckIfPlayerAlive(GameObject player)//Is a Player dead?
    {
        if (GetHealthOfPlayer(player) <= 0)
        {
            Death(player);
        }
    }
    public int GetHealthOfPlayer(GameObject player) //Get The Health value of a player
    {
        return Health[Players.IndexOf(player)];
    }
    public void SetHealthOfPlayer(GameObject player, int value) //z.B. wenn ein spieler getroffen wird und dmg bekommt
    {
        Health[Players.IndexOf(player)] = value;
        CheckIfPlayerAlive(player);
    }

    public void SubstractHealthFromPlayer(GameObject player, int value) //z.B. wenn ein spieler getroffen wird und dmg bekommt
    {
        Health[Players.IndexOf(player)] -= value;
        CheckIfPlayerAlive(player);
    }

    public void AddHealthToPlayer(GameObject player, int value) //z.B. wenn ein spieler geheilt wird
    {
        Health[Players.IndexOf(player)] += value;
    }

    //Kills
    public int GetKillsOfPlayer(GameObject player)
    {
        return Kills[Players.IndexOf(player)];
    }

    public void AddKillsToPlayer(GameObject player, int value = 1)
    {
        Kills[Players.IndexOf(player)] += value;
    }

    public void SubstractKillsFromPlayer(GameObject player, int value = 1)
    {
        Kills[Players.IndexOf(player)] -= value;
    }

    public void SetKillsOfPlayer(GameObject player, int value = 1)
    {
        Kills[Players.IndexOf(player)] = value;
    }

    //Death
    private void Death(GameObject deadPlayer, GameObject killerPlayer = null) //Player dies and and MAYBE another player gets a kill
    {
        if(killerPlayer != null)
        {
            //Add kill to killer
            Kills[Players.IndexOf(killerPlayer)] += 1;
        }
        Deaths[Players.IndexOf(deadPlayer)] += 1;
        //Deactivate deadPlayer
    }

    public int GetDeathsOfPlayer(GameObject player)
    {
        return Deaths[Players.IndexOf(player)];
    }

    public void AddDeathsToPlayer(GameObject player, int value = 1)
    {
        Deaths[Players.IndexOf(player)] += value;
    }

    public void SubstractDeathsFromPlayer(GameObject player, int value = 1)
    {
        Deaths[Players.IndexOf(player)] -= value;
    }

    public void SetDeathsOfPlayer(GameObject player, int value = 1)
    {
        Deaths[Players.IndexOf(player)] = value;
    }
}
