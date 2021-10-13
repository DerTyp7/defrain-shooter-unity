using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
JOIN
1. Wenn ein Spieler joined wird sichergestellt, dass er nicht in der Liste ist (fürs error handling).
2. Dann wird er in die Liste "Players" eingetragen.

LEAVE
1. Index von dem Spieler, in der Liste, wird gesucht
2. Spieler wird aus der Liste entfernt

*/
public class PlayerMaster : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players = new List<GameObject>(); //Contains All Players which are currently connected/in-game
    [SerializeField] private List<int> Health = new List<int>();

    private void Start()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player")); //Add All Player-GameObjects into a List
    }

    //Join
    public void OnPlayerJoin(GameObject player) //When a Player joins
    {
        Debug.Log("Player joined"); //Give Console Feedback
        if (!Players.Contains(player)) //If the Player is NOT in the "Players-List" (For Error Handling)
        {
            Players.Add(player); //Add New Player To List
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
        }
        else
        {
            Debug.LogError("Player not found in Players-list"); //Error, because the Player is NOT in the list -> !critical Anomaly!
        }
    }
}
