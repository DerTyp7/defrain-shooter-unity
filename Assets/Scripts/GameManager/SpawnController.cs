using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    //Spawnpoints
    [Header("Spawnpoints")]
    [SerializeField] List<Spawnpoint> availableSpawns = new List<Spawnpoint>();

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public bool GetAvailableSpawnPoint(int teamID,out Vector3 spawnPosition) 
    {
        
        foreach (Spawnpoint spawn in availableSpawns)
        {
            if (spawn.teamID == teamID && spawn.available && !spawn.blocked)
            {
                spawnPosition = spawn.position;
                spawn.disableForSeconds(15f);
                
                return true;
            }
        }
        spawnPosition = new Vector3();
        return false;
    }
}
