using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Transports;
using MLAPI.Messaging;
using TMPro;

public class PlayerNetworkingScript : NetworkBehaviour
{
    public NetworkVariableInt dice = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J");
            NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.StartClient();
        }
    }



}
