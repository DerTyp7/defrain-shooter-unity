using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
JOIN
1. Wenn ein Spieler joined wird sichergestellt, dass er nicht in der Liste ist (fürs error handling).
2. Dann wird er in die Liste "Players" eingetragen und ein Eintrag in der "health" liste wird mit dem selben index gemacht.

LEAVE
1. Index von dem Spieler, in der Liste, wird gesucht
2. Spieler wird aus der Liste entfernt
3. health index vom spieler wird auch entfernt

*/
public class PlayerMaster : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players = new List<GameObject>();
    [SerializeField] private List<int> Health = new List<int>();

    private void Start()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void OnPlayerJoin(GameObject player)
    {
        Debug.Log("Player joined");
        if (!Players.Contains(player))
        {
            Players.Add(player);
            Debug.Log("Player added to list");
        }
        else
        {
            Debug.LogError("Player already exits in list");
        }
    }

    public void OnPlayerLeave(GameObject player)
    {
        Debug.Log("Player left");
        if (Players.Contains(player))
        {
            Players.Remove(player);
        }
        else
        {
            Debug.LogError("Player not found in Players-list");
        }
    }
}
