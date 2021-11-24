using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    public int currentWeaponIndex = 0;
    public GameObject[] activeWeapons;
    private int counter = 0;

    [SerializeField] Camera cam;

    private void Awake()
    {
        activeWeapons = new GameObject[4];
        for(int i = 0; i<4; i++)
        {
            activeWeapons[i] = null;
        }
    }

    void Update() {
        if (isLocalPlayer) {
            counter = 0;
            if(Input.GetAxis("Mouse ScrollWheel") > 0f && false){ // Scroll up
                do
                {
                    if (currentWeaponIndex <= 0)
                    {
                        currentWeaponIndex = activeWeapons.Length - 1;
                    }
                    else
                    {
                        currentWeaponIndex--;
                    }
                    counter++;
                    //Debug.Log(activeWeapons[currentWeaponIndex]);
                } while (activeWeapons[currentWeaponIndex] == null);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && false){ // Scroll down
                do
                {
                    if (currentWeaponIndex >= activeWeapons.Length - 1)
                    {
                        currentWeaponIndex = 0;
                    }
                    else
                    {
                        currentWeaponIndex++;
                    }
                    counter++;
                    //Debug.Log(activeWeapons[currentWeaponIndex]);
                } while (activeWeapons[currentWeaponIndex] == null);
                
            }

            if (Input.GetButton("Interact")) // e
            {
                CmdPickupWeapon();
            
            }else if (Input.GetButton("Drop")) // q Droping weapon
            {
                activeWeapons[currentWeaponIndex] = null;
            }
        }
    }

    [Command]
    private void CmdPickupWeapon() {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) 
        {
            if (hit.transform.tag == "Weapon") // If Object is a weapon and the weapon is not in the current active weapons
            {
                
                switch (hit.transform.GetComponent<Weapon>().WeaponKind.ToString()) // Adding weapon to inventory slot
                {
                    case "Rifle": activeWeapons[0] = hit.transform.gameObject; break;
                    case "Pistole": activeWeapons[1] = hit.transform.gameObject; break;
                    case "Knife": activeWeapons[2] = hit.transform.gameObject; break;
                    case "Grenade": activeWeapons[3] = hit.transform.gameObject; break;
                    default: break;
                }
            }
        }
    }

}
