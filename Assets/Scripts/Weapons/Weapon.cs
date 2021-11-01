using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum weaponKinds
    {
        Rifle, Pistole, Knife, Granade
    }
    [SerializeField] weaponKinds weaponKind;
    [SerializeField] bool active = false;
    [SerializeField] float damage = 0;
    [SerializeField] float firerate = 0;
    [SerializeField] float recoilStrength = 0;
    [SerializeField] int currentAmmunition = 0;
    [SerializeField] int magazinSize = 0;
    [SerializeField] int totalAmmunition = 0;
    [SerializeField] ParticleSystem flash;
    [SerializeField] GameObject bulletExit;

    private bool allowShoot = true;

    public bool Active { get => active; set => active = value; }
    public weaponKinds WeaponKind { get => weaponKind; set => weaponKind = value; }
   

    private void Start()
    {
        currentAmmunition = magazinSize;
    }
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire") && allowShoot && currentAmmunition > 0)
        {
            fire();
            StartCoroutine(fireRate());
            currentAmmunition--;
        }
        if (Input.GetButton("Reload"))
        {
            if (allowShoot && totalAmmunition > 0)
            {
                allowShoot = false;
                int dif = magazinSize - currentAmmunition; 

                if(totalAmmunition >= dif) {
                    currentAmmunition += dif;
                    totalAmmunition -= dif;
                }
                else{
                    currentAmmunition += totalAmmunition;
                    totalAmmunition = 0;
                }
                allowShoot = true;
            }
        }
        if (Input.GetButton("Aim"))
        {
            
        }
        
    }
    private void fire()
    {
        allowShoot = false;
        flash.Play();
        RaycastHit hit; 
        
        if(Physics.Raycast(bulletExit.transform.position,bulletExit.transform.forward, out hit))
        {
            Debug.DrawLine(bulletExit.transform.position, hit.point);
        }
        
    }

    IEnumerator fireRate()
    {
        allowShoot = false;
        yield return new WaitForSeconds(firerate);
        allowShoot = true;
    }

}
