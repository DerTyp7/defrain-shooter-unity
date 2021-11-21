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
                int nextActive = SearchForNext(activeWeapons, lastWeaponIndex, -1);
                if (nextActive != -1) { // -1 no next found
                    currentWeaponIndex = nextActive;
                    activeWeapons[currentWeaponIndex].SetActive(true);
                    // play weapon switch animation
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f){ // Scroll down
                lastWeaponIndex = currentWeaponIndex;
                int nextActive = SearchForNext(activeWeapons, lastWeaponIndex, 1);
                if (nextActive != -1) { // -1 no next found
                    currentWeaponIndex = nextActive;
                    activeWeapons[currentWeaponIndex].SetActive(true);
                    // play weapon switch animation
                }
            }
            
            if (Input.GetButtonDown("Interact")) // e
            {
                CmdPickupWeapon();
            
            }else if (Input.GetButtonDown("Drop")) // q Droping weapon
            {
                if(activeWeapons[currentWeaponIndex] != null)
                {
                    Rigidbody rigid = activeWeapons[currentWeaponIndex].GetComponent<Rigidbody>();
                    rigid.useGravity = true;
                    rigid.isKinematic = false;
                    activeWeapons[currentWeaponIndex].transform.position = cam.transform.position;
                    activeWeapons[currentWeaponIndex].GetComponent<BoxCollider>().enabled = true;
                    rigid.velocity = cam.transform.forward * 10 + cam.transform.up * 2;
                    activeWeapons[currentWeaponIndex].gameObject.transform.SetParent(null);
                    activeWeapons[currentWeaponIndex] = null;

                    int nextActive = SearchForNext(activeWeapons, currentWeaponIndex, 1);
                    if(nextActive != -1) // -1 no next found
                    {
                        lastWeaponIndex = currentWeaponIndex;
                        currentWeaponIndex = nextActive;
                        activeWeapons[currentWeaponIndex].SetActive(true);
                    }
                    
                }

            }

        }

    }

    private int SearchForNext(List<GameObject> l, int lastActive = 0, int direction = 1) {
        int size = l.Count-1;
        bool condition = true;
        if (lastActive <= -1) { lastActive = size; }
        if(lastActive >= l.Count) { lastActive = 0; }
        for (int i = lastActive+direction; condition; i+= direction) {
            if (i >= l.Count-1) { i = 0; size = lastActive+1;  }
            else if(i < 0) { i = size; size = -1;  }
            if (l[i] != null) {
                if(l[lastActive] != null) { l[lastActive].SetActive(false); }
                return i; 
            }
            if (direction == 1) {
                if (i < size + 1) { condition = true; }
                else { condition = false; }
            }else if(direction == -1) {
                if (i > size + 1) { condition = true; }
                else { condition = false; }
            }
        }
        return -1;
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
