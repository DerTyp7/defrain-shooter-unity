using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private GameObject gun;
    void Start()
    {
        anim = gun.GetComponent<Animator>();
    }

    void OnSwitchWeapon(GameObject newGun)
    {
        gun = newGun;
        anim = gun.GetComponent<Animator>();
    }
    public void StartShootAnimation(float timeInSeconds) 
    {
        anim.PlayInFixedTime("Shoot", 0, timeInSeconds);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
