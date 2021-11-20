using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    public int currentWeaponIndex = 0;
    private int lastWeaponIndex = 0;
    private int counter = 0;
    public List<GameObject> activeWeapons = new List<GameObject>();


    [SerializeField] GameObject gunHolster;
    [SerializeField] Camera cam;

    private void Awake()
    {
        for(int i = 0; i<4; i++)
        {
            activeWeapons.Add(null);
        }
    }

    void Update() {
        if (isLocalPlayer) {
            if(Input.GetAxis("Mouse ScrollWheel") > 0f){ // Scroll up
                lastWeaponIndex = currentWeaponIndex;
                counter = 0;
                do {
                    if (currentWeaponIndex <= 0) {
                        currentWeaponIndex = activeWeapons.Count - 1;
                    } else {
                        currentWeaponIndex--;
                    }
                    counter++;
                    if (counter > 10) { break; }
                } while (activeWeapons[currentWeaponIndex] == null);
                if (lastWeaponIndex != currentWeaponIndex && activeWeapons[currentWeaponIndex] != null)
                {
                    // play switch animation or move weapon (hands) down
                    foreach (GameObject obj in activeWeapons)
                    { // Disable all weapons
                        if (obj != null) { obj.SetActive(false); }
                    }
                    Debug.Log("Set Active (" + currentWeaponIndex + "): " + activeWeapons[currentWeaponIndex].name);
                    activeWeapons[currentWeaponIndex].SetActive(true);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f){ // Scroll down
                lastWeaponIndex = currentWeaponIndex;
                counter = 0;
                do {
                    if (currentWeaponIndex >= activeWeapons.Count - 1) {
                        currentWeaponIndex = 0;
                    } else {
                        currentWeaponIndex++;
                    }
                    counter++;
                    if(counter > 10) { break; }
                } while (activeWeapons[currentWeaponIndex] == null);
                if (lastWeaponIndex != currentWeaponIndex && activeWeapons[currentWeaponIndex] != null)
                {
                    // play switch animation or move weapon (hands) down
                    foreach (GameObject obj in activeWeapons)
                    { // Disable all weapons
                        if (obj != null) { obj.SetActive(false); }
                    }
                    Debug.Log("Set Active (" + currentWeaponIndex + "): " + activeWeapons[currentWeaponIndex].name);
                    activeWeapons[currentWeaponIndex].SetActive(true);
                }
            }
            
            if (Input.GetButtonDown("Interact")) // e
            {
                CmdPickupWeapon();
            
            }else if (Input.GetButtonDown("Drop")) // q Droping weapon
            {
                // WENN GEDROPT WIRD MUSS DIE NÄCHSTE AKTIVE GSUCHT WERDEn
                if(activeWeapons[currentWeaponIndex] != null)
                {
                    activeWeapons[currentWeaponIndex].GetComponent<Rigidbody>().useGravity = true;
                    activeWeapons[currentWeaponIndex].GetComponent<Rigidbody>().isKinematic = false;
                    activeWeapons[currentWeaponIndex].transform.position = cam.transform.position;
                    activeWeapons[currentWeaponIndex].GetComponent<BoxCollider>().enabled = true;
                    activeWeapons[currentWeaponIndex].GetComponent<Rigidbody>().velocity = cam.transform.forward * 10 + cam.transform.up * 2;
                    activeWeapons[currentWeaponIndex].gameObject.transform.SetParent(null);
                    activeWeapons[currentWeaponIndex] = null;
                }
            }
        }

    }

    [Command]
    private void CmdPickupWeapon() {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) 
        {
            if (hit.transform.tag == "Weapon") // If Object is a weapon and the weapon is not in the current active weapons
            {
                foreach (GameObject obj in activeWeapons) { // Disable all weapons
                    if (obj != null) { obj.SetActive(false); }
                }
                hit.transform.parent = gunHolster.transform; // Parent weapon to gunHolster
                hit.rigidbody.isKinematic = true;
                hit.rigidbody.useGravity = false;
                hit.transform.GetComponent<BoxCollider>().enabled = false; // Disable Boxcollider 
                hit.transform.position = cam.transform.position;
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
