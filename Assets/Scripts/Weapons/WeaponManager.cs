using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int currentWeaponIndex = 0;
    public List<GameObject> allWeapons = new List<GameObject>();
    public GameObject[] activeWeapons;

    [SerializeField] GameObject cam;

    private void Awake()
    {
        activeWeapons = new GameObject[4];
    }

    void Update() { 
        if(Input.GetAxis("Mouse ScrollWheel") > 0f){ // Scroll up
            if (currentWeaponIndex <= 0) 
            { currentWeaponIndex = activeWeapons.Length; }
            else { currentWeaponIndex--; }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f){ // Scroll down
            if (currentWeaponIndex >= activeWeapons.Length) 
            { currentWeaponIndex = 0; }
            else {  currentWeaponIndex++; }
        }
        if (Input.GetButton("Interact")) // e
        { 
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
            {
                Debug.DrawLine(cam.transform.position, hit.point);
                if (allWeapons.Contains(hit.transform.gameObject) && !searchInArray(activeWeapons, hit.transform.gameObject)) // If Object is a weapon and the weapon is not in the current active weapons
                {
                    switch (hit.transform.GetComponent<Weapon>().WeaponKind.ToString()) // Adding weapon to inventory slot
                    {
                        case "Rifle": activeWeapons[0] = hit.transform.gameObject; break;
                        case "Pistole": activeWeapons[1] = hit.transform.gameObject; break;
                        case "Knife": activeWeapons[2] = hit.transform.gameObject; break;
                        case "Granade": activeWeapons[3] = hit.transform.gameObject; break;
                        default: break;
                    }                    
                }                
            }
        }
        if (Input.GetButton("Drop")) // q Droping weapon
        {
            activeWeapons[currentWeaponIndex] = null;
        }
    }


    private bool searchInArray(GameObject[] arr, GameObject searchObj)
    {
        foreach(GameObject obj in arr)
        {
            if (obj == searchObj) return true;
        }
        return false;
    }

}
