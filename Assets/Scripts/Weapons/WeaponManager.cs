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
                activeWeapons[currentWeaponIndex].SetActive(false);
                switchWeapon(-1);
                activeWeapons[currentWeaponIndex].SetActive(true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // Scroll down
                lastWeaponIndex = currentWeaponIndex;
                activeWeapons[currentWeaponIndex].SetActive(false);
                switchWeapon(1);
                activeWeapons[currentWeaponIndex].SetActive(true);
            }
            if (Input.GetButtonDown("Interact")) { // e 
                    PickupWeapon();
            }else if (Input.GetButtonDown("Drop")) { // q Droping weapon 
                if (activeWeapons[currentWeaponIndex] != null) {
                    dropWeapon(activeWeapons[currentWeaponIndex].GetComponent<Weapon>().DropForce); // Throws weapon away
                    switchWeapon(1);
                    activeWeapons[currentWeaponIndex].SetActive(true);
                }
            }
        }
    }
    

    public bool switchWeapon(int direction) {
        // Get next active weapon index
        int nextActive = searchForNext(activeWeapons, lastWeaponIndex, direction);

        currentWeaponIndex = nextActive;
            
        procedualAnimationController.OnSwitchWeapon(activeWeapons[currentWeaponIndex]);
        shoot.setWeapon(activeWeapons[currentWeaponIndex]);
        Weapon weaponData = activeWeapons[currentWeaponIndex].GetComponent<Weapon>();
        procedualAnimationController.GunRightHandREF = weaponData.GunRightREF;
        procedualAnimationController.GunLeftHandREF = weaponData.GunLeftREF;
        // play weapon switch animation

        return false;
    }

    private int searchForNext(List<GameObject> l, int lastActive = 0, int direction = 1)
    {
        int current = lastActive + direction;

        for (int trys = 0; trys < l.Count; trys++)
        {
            //Check if you are at the start or end of the list and loop around accordingly
            if (current < 0) current = l.Count - 1; 
            else if (current >= l.Count) current = 0;
            
            //Check if in the current position is a gun or not 
            if (l[current] != null) 
            {
                return current;
            }
            //if there is no gun go to the next
            current = current + direction;
        }
        //If no next gun can be found return the index of the gun that was already selected
        return lastActive;
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
                SetAllColliderStatus(hit.transform.gameObject, false); // Disable all Collider
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
        if (activeWeapons[index] != null) {
            dropWeapon(activeWeapons[currentWeaponIndex].GetComponent<Weapon>().DropForce); // Throws weapon away
        }
        activeWeapons[index] = hit.transform.gameObject;
        activeWeapons[index].SetActive(true);
        // \/ Same as in switchWeapon()
        currentWeaponIndex = index;
        procedualAnimationController.OnSwitchWeapon(activeWeapons[currentWeaponIndex]);
        shoot.setWeapon(activeWeapons[currentWeaponIndex]);
        Weapon weaponData = activeWeapons[currentWeaponIndex].GetComponent<Weapon>();
        procedualAnimationController.GunRightHandREF = weaponData.GunRightREF;
        procedualAnimationController.GunLeftHandREF = weaponData.GunLeftREF;
        return true;
    }

    public bool dropWeapon(float dropForce) {
        if(currentWeaponIndex != 2) {
            GameObject currentWeapon = activeWeapons[currentWeaponIndex];
            currentWeapon.SetActive(true);
            Rigidbody rigid = currentWeapon.GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.velocity = cam.transform.forward * dropForce + cam.transform.up * 2;
            SetAllColliderStatus(currentWeapon, true); // Activate all Collider
            currentWeapon.gameObject.transform.SetParent(null);
            activeWeapons[currentWeaponIndex] = null;
            return true;
        }
        else {
            return false;
        }
    }
    
    // Set status to all colliders on a gameobject
    private void SetAllColliderStatus(GameObject obj, bool newStatus) {
        // For every collider on gameobject 
        foreach(Collider col in obj.GetComponents<Collider>()) {
            // set newStatus (enable, disable)
            col.enabled = newStatus;
        }
    }

}
