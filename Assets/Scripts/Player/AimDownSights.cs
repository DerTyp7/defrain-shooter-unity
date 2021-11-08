using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    [SerializeField] float aimSpeed = 0.01f;
    [SerializeField][Range(0,1)] float aimVal = 0;
    [SerializeField] private GameObject gun;
    [SerializeField] GameObject AimPoint;
    [SerializeField] GameObject HoldPoint;
    public bool isAiming = false;
    bool ADS() 
    {
        
        return true;
    }
    private void Update()
    {
        Debug.Log(Input.GetButton("Aim"));
        if (Input.GetButton("Aim")) 
        {
            isAiming = true;
            aimVal += aimSpeed;
        } 
        else 
        {
            isAiming = false;
            aimVal -= aimSpeed;
        }
        aimVal = Mathf.Clamp(aimVal,0,1);
        
        gun.transform.position = Vector3.Lerp(HoldPoint.transform.position, AimPoint.transform.position, aimVal);
    }
}
