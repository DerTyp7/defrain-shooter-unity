using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Manager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        //Debug.Log(conn.identity.gameObject.GetComponent<Player>().username);
        
        //conn.identity.gameObject.GetComponent<Player>().username = "Test";
    }
}
