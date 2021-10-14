using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float SyncIntervalSeconds = 5.0f;
    [SerializeField] GameObject GameManager;

    private PlayerMaster playerMaster;

    private void Start()
    {
        playerMaster = GameManager.GetComponent<PlayerMaster>();
        InvokeRepeating("Sync", 3.0f, SyncIntervalSeconds);
    }


    private void Sync()
    {
        Debug.Log("Sync");
        health = playerMaster.GetHealthOfPlayer(gameObject);
    }

    public void SubstractHealth(int value)
    {
        health -= value;
    }

    public void AddHealth(int value)
    {
        health += value;
    }
}
