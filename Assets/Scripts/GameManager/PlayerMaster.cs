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
    [Header("PlayerMaster")]
    [SerializeField] private List<ServerPlayer> Players = new List<ServerPlayer>(); //Contains All Players which are currently connected/in-game

    //JUST FOR DEBUG
    [SerializeField] private GameObject TestPlayer;

    private void Update()
    {
        //JUST FOR DEBUG
        Players[0].AddKills(1);
    }


    private void Start()
    {
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            Players.Add(new ServerPlayer(p));
        }


        InvokeRepeating("TestDamage", 3.0f, 3f);
    }

    public void TestDamage()
    {
        Players[0].RemoveHealth(10);
    }
    //Join
    public void OnPlayerJoin(GameObject player) //When a Player joins
    {
        foreach (ServerPlayer p in Players)
        {
            if (p.Player == player)
            {
                Debug.Log("Joined Player already exits");
                return;
            }
        }
        Players.Add(new ServerPlayer(player));
    }

    //Leave
    public void OnPlayerLeave(GameObject player) //When a Player leaves
    {
        foreach (ServerPlayer p in Players)
        {
            if (p.Player == player)
            {
                Players.Remove(p);
            }
        }
    }

    public int SyncHealth(GameObject player)
    {
        foreach (ServerPlayer p in Players)
        {
            if (p.Player == player)
            {
                return p.GetHealth();
            }
        }
        return -1;
    }
}
