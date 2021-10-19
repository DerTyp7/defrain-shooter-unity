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
    [SerializeField] ParticleSystem smoke;
    [SerializeField] GameObject bulletExit;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject model;

    private bool allowShoot = true, isReloading = false;

    public bool Active { get => active; set => active = value; }
    public weaponKinds WeaponKind { get => weaponKind; set => weaponKind = value; }
   
    //Animator anim;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        currentAmmunition = magazinSize;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) // Not Working
        {
            transform.rotation = Quaternion.Euler(0f, hit.point.y, 0f);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire") && allowShoot && currentAmmunition > 0)
        {
            //anim.Play("USP_Shooting");
            StartCoroutine(fireRate());
            fire();
            currentAmmunition--;
        }
        if (Input.GetButton("Reload"))
        {
            if (isReloading == false && totalAmmunition > 0)
            {
                isReloading = true;
                int dif = magazinSize - currentAmmunition; 

                if(totalAmmunition >= dif) {
                    currentAmmunition += dif;
                    totalAmmunition -= dif;
                }
                else{
                    currentAmmunition += totalAmmunition;
                    totalAmmunition = 0;
                }
                isReloading = false;
            }
        }
        if (Input.GetButton("Aim")) // Not working properly, maybe Animations are not correct to use
        {
            //anim.Play("USP_Aim");
        }
        
    }
    private void fire()
    {
        allowShoot = false;
        flash.Play();
        //smoke.Play();
        RaycastHit hit; 
        
        if(Physics.Raycast(bulletExit.transform.position,bulletExit.transform.forward, out hit))
        {
            Debug.DrawLine(bulletExit.transform.position, hit.point);
            /*if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * 80);
            }
            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));*/
        }
        
    }

    IEnumerator fireRate()
    {
        allowShoot = false;
        yield return new WaitForSeconds(firerate);
        allowShoot = true;
    }

}
