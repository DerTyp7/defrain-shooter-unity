using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{

    [Header("GameObjects")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject gunHolder;
    [SerializeField] private GameObject gunPositionObj;
    [SerializeField] private GameObject gunRotationObj;
    

    [Header("Settings")]
    [SerializeField] bool positionRecoil = true;
    [SerializeField] bool rotationRecoil = true;

    [Header("Position Settings")]
    [SerializeField] float positionMultX = 25f;
    [SerializeField] float positionMultY = 25f;
    [SerializeField] float positionMultZ = 25f;

    [Header("Rotation Settings")]
    [SerializeField] bool rotX = true;
    [SerializeField] float rotationMultX = 25f;
    [SerializeField] bool rotY = true;
    [SerializeField] float rotationMultY = 25f;
    [SerializeField] bool rotZ = true;
    [SerializeField] float rotationMultZ = 15f;



    [SerializeField] float returnForce = 0.006f;
    [SerializeField] float impulsForce = 0.025f;
    [SerializeField] float maxRecoil = 0.1f;

    private Animator anim;

    Vector3 startPos,startRot;

    float zOffset = 0f;
    float zVelocity = 0f;

    int recoilCounter = 0;

    //Has to be called once at the beginning and then again when switching guns
    public void OnSwitchWeapon(float fireRate)
    {
        //gun = newGun;
        anim = gun.GetComponent<Animator>();
        anim.SetFloat("ShootSpeed",1f/(60f/fireRate));
        startPos = gunPositionObj.transform.localPosition;
        startRot = gunRotationObj.transform.localRotation.eulerAngles;
    }


    public void recoil(GameObject gun, float force)
    {
        //Play the animation
        anim.Play("Shoot");

        //Add force for the recoil
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

        //Position recoil
        if (positionRecoil)
        {
            gunPositionObj.transform.localPosition = startPos + new Vector3(
                positionMultX * zOffset * ((Mathf.PerlinNoise(Time.time * 1f + 10f, 1f) - 0.5f) * 2f),
                positionMultY * zOffset * ((Mathf.PerlinNoise(Time.time * 2f + 20f, 2f) - 0.5f) * 2f),
                positionMultZ* zOffset * ((Mathf.PerlinNoise(Time.time * 3f + 30f, 3f) - 0.5f) * 2f));
        }
        else 
        {
            gunPositionObj.transform.localPosition = startPos;
        }

        //Rotation recoil
        if (rotationRecoil)
        {
            int xLock = 0;
            int yLock = 0;
            int zLock = 0;

            if (rotX) xLock = 1;
            if (rotY) yLock = 1;
            if (rotZ) zLock = 1;

            gunRotationObj.transform.localRotation = Quaternion.Euler(
                startRot.x + xLock * rotationMultX * zOffset * ((Mathf.PerlinNoise(Time.time * 3f + 30f, 4f) - 0.5f) * 2f),
                startRot.y + yLock * rotationMultY * zOffset * ((Mathf.PerlinNoise(Time.time * 2f + 10f, 3f) - 0.5f) * 2f),
                startRot.z + zLock * rotationMultZ * zOffset * ((Mathf.PerlinNoise(Time.time * 1.5f, 2f) - 0.5f) * 2f));
        }
        else
        {
            gunRotationObj.transform.localRotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z);
        }
            

    }
}
