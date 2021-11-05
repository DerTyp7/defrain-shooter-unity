using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool subtractAmmunition(Weapon weapon)
    {
        if (weapon.CurrentAmmunition > 0) {
            weapon.CurrentAmmunition -= weapon.RoundsPerShot;
            Debug.Log(weapon.CurrentAmmunition + " - " + weapon.RoundsPerShot);
            return true;
        }
        return false;
    }

    public bool reloadWeapon(Weapon weapon)
    {
        if (weapon.AllowAction && weapon.TotalAmmunition > 0) {
            weapon.AllowAction = false;
            int dif = weapon.MagazinSize - weapon.CurrentAmmunition;

            if (weapon.TotalAmmunition >= dif) {
                weapon.CurrentAmmunition += dif;
                weapon.TotalAmmunition -= dif;
            }
            else {
                weapon.CurrentAmmunition += weapon.TotalAmmunition;
                weapon.TotalAmmunition = 0;
            }
            weapon.AllowAction = true;
            Debug.Log("Reloaded");
            return true;
        }
        return false;
    }
}
