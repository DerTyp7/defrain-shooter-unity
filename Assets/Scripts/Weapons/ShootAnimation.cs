using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private GameObject gun;

    public void OnSwitchWeapon(float fireRate)
    {
        //gun = newGun;
        anim = gun.GetComponent<Animator>();
        anim.SetFloat("ShootSpeed",1f/(60f/fireRate));
    }
    public void StartShootAnimation(float timeInSeconds) 
    {
        Debug.Log(anim.GetFloat("ShootSpeed"));
        anim.Play("Shoot");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
