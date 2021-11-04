using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject muzzle;

    private void Start()
    {

    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //CmdFireBullet();
                RpcOnFire();
                Debug.Log("Hit Left Mouse  ");
            }
        }
    }
    private void OnDrawGizmos()
    {

        if (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            Gizmos.color = Color.red;
        }
        else 
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(muzzle.transform.position, muzzle.transform.forward);

    }
    
    [Command]
    // This code will be executed on the server.
    private void CmdFireBullet()
    {
        GameObject dedplayer;
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit))
        {

            if (hit.transform.gameObject.GetComponent<Player>() != null)
            {
                Debug.Log("Hit player:  " + hit.transform.gameObject.name);
                dedplayer = hit.transform.gameObject;
                //dedplayer.GetComponent<Player>().health -= 20;
                dedplayer.GetComponent<Player>().RemoveHealth(20);

            }
        }
    }


    [Client]
    // This code will be executed on the Client.
    void RpcOnFire()
    {
        CmdFireBullet();

    }
}
