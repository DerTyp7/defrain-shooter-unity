using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject gun;
    private Transform startTransform;
    Vector3 startPos;
    Vector3 startRot;
    [SerializeField]  float zOffset = 0f;
    [SerializeField]  float returnForce = 0.06f;
    [SerializeField]  float impulsForce = 0.2f;
    [SerializeField] float maxRecoil = 0.5f;
    private int recoilCounter = 0;

    float zVelocity = 0f;


    public void OnSwitchWeapon(float fireRate)
    {
        //gun = newGun;
        anim = gun.GetComponent<Animator>();
        anim.SetFloat("ShootSpeed",1f/(60f/fireRate));
        startPos = gun.transform.localPosition;
        startRot = gun.transform.localRotation.eulerAngles;

    }


    public void recoil(GameObject gun, float force)
    {
        anim.Play("Shoot");
        recoilCounter++;
    }


    void FixedUpdate()
    {
        //Apply recoil based on the number of shots fired
        for (int i = 0; i < recoilCounter; i++) 
        {
            zVelocity -= impulsForce * 0.9f + impulsForce * 0.1f  * Mathf.PerlinNoise(i,1f);
        }
        recoilCounter = 0;


        zOffset += zVelocity;

        if (zOffset > 0)
        {
            zOffset = 0f;
            zVelocity = 0f;
        }
        else if (zOffset < 0)
        {
            zVelocity += returnForce * 0.9f + returnForce * 0.1f * Mathf.PerlinNoise(Time.time,1f);
            
        }
        zOffset = Mathf.Clamp(zOffset,-maxRecoil * 0.5f + -maxRecoil * 0.5f * Mathf.PerlinNoise(Time.time * 1000,1),0);    
        gun.transform.localPosition = startPos + new Vector3(0,0,zOffset);
        gun.transform.localRotation = Quaternion.Euler(startRot.x,startRot.y,startRot.z + zOffset * 50f);
    }
}
