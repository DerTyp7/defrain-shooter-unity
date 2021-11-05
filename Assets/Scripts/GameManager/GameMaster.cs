using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Erstellung von Teams
// Auflistung von den Spielern
// Verwaltung der Spieler und Teams
// 

public class GameMaster : MonoBehaviour
{
    [Header("GameMaster")]
    [SerializeField] private List<Player> Players = new List<Player>(); 
    [SerializeField] private int countOfRounds = 10;

    public GameObject localPlayer;

    private void Update()
    {
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



}
