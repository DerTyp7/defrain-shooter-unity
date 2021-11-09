using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Headbob : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ShootAnimation gunAnimation;


    [SerializeField] private float posCheckDistance = 0.01f;
    [SerializeField] private float checkDist = 0.0f;
    private float currentDist = 0;

    [Header("Step Settings")]
    [SerializeField] private float stepAmplitudeWalking;
    [SerializeField] private float stepAmplitudeSprinting;
    [SerializeField] [Range(0.01f, 10.0f)] private float stepFrequency;
    [SerializeField] private Transform Neck;

    private Vector3 lastPos;
    private Vector3 startPos;
    private Vector3 newPos;
    private float oldDist = 0;
    private float lerpVal = 0;


    [Header("Gun Settings")]
    [SerializeField] GameObject gunRotation;
    [SerializeField] float rotationMultiplier = 0.1f;



    private void Start()
    {
        lastPos = this.transform.position;
        //startPos = this.transform.position;
    }

    private void Update()
    {
        
        float amplitude;
        if (playerController.isGrounded)
        {
            lerpVal = 0;
            float dist = Vector3.Distance(lastPos, this.transform.position);
            if (playerController.isSprinting)
                amplitude = stepAmplitudeSprinting;
            else
                amplitude = stepAmplitudeWalking;

            if (dist > posCheckDistance)
            {
                currentDist += dist;
                lastPos = this.transform.position;
            }
            else
            {
                checkDist = currentDist + dist;
            }
            gunAnimation.gunSideSwey(getSin(amplitude, stepFrequency/2, checkDist),playerController.inputDirection.magnitude);
            newPos = new Vector3(getSin(amplitude / 2, stepFrequency, checkDist), getSin(amplitude, stepFrequency, checkDist), 0);
            Neck.localPosition = newPos;
        }
        else
        {
            Neck.localPosition = Vector3.zero;            
        }
        
    }

    private float getSin(float multiplier, float devisor,float x)
    {
        return multiplier * Mathf.Sin((x/3.14f) * 10 * devisor);
    }
    
}
