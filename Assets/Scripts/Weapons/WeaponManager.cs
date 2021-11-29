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
    private ProcedualAnimationController procedualAnimationController;

    [SerializeField] Shoot shoot;
    [SerializeField] GameObject gunHolster;
    [SerializeField] Camera cam;



    private void Awake()
    {
        procedualAnimationController = GetComponent<ProcedualAnimationController>();
    }

    void Update() {
        if (isLocalPlayer) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // Scroll up
                lastWeaponIndex = currentWeaponIndex;
                switchWeapon(-1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // Scroll down
                lastWeaponIndex = currentWeaponIndex;
                switchWeapon(1);
            }

            if (Input.GetButtonDown("Interact")) { // e 
                    PickupWeapon();

            }else if (Input.GetButtonDown("Drop")) { // q Droping weapon 
                if (activeWeapons[currentWeaponIndex] != null) {
                    dropWeapon(); // Throws weapon away
                    switchWeapon(1);
                }
            }
        }
    }
    

    private bool switchWeapon(int direction) {
        int nextActive = searchForNext(activeWeapons, lastWeaponIndex, direction);
        if (nextActive != -1) { // -1 no next found
            currentWeaponIndex = nextActive;
            activeWeapons[currentWeaponIndex].SetActive(true);
            procedualAnimationController.OnSwitchWeapon(activeWeapons[currentWeaponIndex]);
            shoot.setWeapon(activeWeapons[currentWeaponIndex]);
            Weapon weaponData = activeWeapons[currentWeaponIndex].GetComponent<Weapon>();
            procedualAnimationController.GunRightHandREF = weaponData.GunRightREF;
            procedualAnimationController.GunLeftHandREF = weaponData.GunLeftREF;
            // play weapon switch animation
        }

        return false;
    }
    private int searchForNext(List<GameObject> l, int lastActive = 0, int direction = 1) {
        int size = l.Count;
        bool condition = true;
        int counter = 0;
        foreach (GameObject obj in l) { if(obj == null) { counter++;  } }
        if(counter < 4) {
            if (lastActive <= -1) { lastActive = size; }
            if (lastActive >= l.Count) { lastActive = 0; }
            for (int i = lastActive + direction; condition; i += direction) {
                if (i >= l.Count) { 
                    i = 0; size = lastActive; 
                }
                else if (i < 0) {
                    i = size - 1; size = -1; 
                }

                Debug.Log("1 " + i);

                if (l[i] != null) {
                    if (l[lastActive] != null) { 
                        l[lastActive].SetActive(false); 
                    }
                    return i;
                }

                Debug.Log("2 " + i);

                if (direction == 1) {
                    if (i <= size - 1) { condition = true; }
                    else { condition = false; }
                } else if (direction == -1) {
                    if (i >= size - 1) { condition = true; }
                    else { condition = false; }
                }
            }
        }
        
        return -1;
    }

    public GameObject getCurrentWeapon() {
        return activeWeapons[currentWeaponIndex].gameObject;
    }

    private void PickupWeapon() {
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
                switch (hit.transform.GetComponent<Weapon>().WeaponKind.ToString()) { // Adding weapon to inventory slot
                    case "Rifle": putWeaponInArray(0, hit); break;
                    case "Pistole": putWeaponInArray(1, hit); break;
                    case "Knife": putWeaponInArray(2, hit); break;
                    case "Grenade": putWeaponInArray(3, hit); break;
                    default: break;
                }
            }
        }
    }

    private bool putWeaponInArray(int index, RaycastHit hit) {
        if (activeWeapons[currentWeaponIndex] != null) {
            dropWeapon(); // Throws weapon away
        }
        activeWeapons[index] = hit.transform.gameObject;
        activeWeapons[index].SetActive(true);
        currentWeaponIndex = index;
        procedualAnimationController.OnSwitchWeapon(activeWeapons[currentWeaponIndex]);
        shoot.setWeapon(activeWeapons[currentWeaponIndex]);
        Weapon weaponData = activeWeapons[currentWeaponIndex].GetComponent<Weapon>();
        procedualAnimationController.GunRightHandREF = weaponData.GunRightREF;
        procedualAnimationController.GunLeftHandREF = weaponData.GunLeftREF;
        return true;
    }

    private bool dropWeapon() {
        if(currentWeaponIndex != 2) {
            GameObject currentWeapon = activeWeapons[currentWeaponIndex];
            currentWeapon.SetActive(true);
            Rigidbody rigid = currentWeapon.GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.velocity = cam.transform.forward * 10 + cam.transform.up * 2;
            currentWeapon.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.transform.SetParent(null);
            activeWeapons[currentWeaponIndex] = null;

            return true;
        }
        else {
            return false;
        }
        
    }

}
