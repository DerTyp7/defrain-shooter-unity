using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject muzzle;
    [SerializeField] ShootAnimation shootAnim;
    [SerializeField] float fireRate;

    private void Start()
    {
        if (isLocalPlayer)
        {
            shootAnim.OnSwitchWeapon(fireRate);
        }
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //CmdFireBullet();
                //RpcOnFire();
                CmdFireBullet();
            }
        }
    }
    
    [Command]
    // This code will be executed on the server.
    private void CmdFireBullet()
    {
        GameObject dedplayer;
        RaycastHit hit;

        shootAnimation();

        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Player>() != null)
            {
                dedplayer = hit.transform.gameObject;
                dedplayer.GetComponent<Player>().RemoveHealth(20);
            }
        }
    }


    [Client]
    // This code will be executed on the Client.
    void shootAnimation()
    {
        shootAnim.StartShootAnimation(fireRate);

    }
}