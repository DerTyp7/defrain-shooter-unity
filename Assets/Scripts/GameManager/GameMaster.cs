using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// Erstellung von Teams
// Auflistung von den Spielern
// Verwaltung der Spieler und Teams
// 

public class GameMaster : MonoBehaviour
{
    [Header("GameMaster")]
    [SerializeField] private List<Player> Players = new List<Player>();
    public GameObject localPlayer;
    private void Start()
    {
    }

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
