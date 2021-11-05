using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Headbob : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController;


    [SerializeField] private float posCheckDistance = 0.01f;
    [SerializeField] private float checkDist = 0.0f;
    private float currentDist = 0;

    [Header("Step Settings")]
    [SerializeField] private float stepAmplitudeWalking;
    [SerializeField] private float stepAmplitudeSprinting;
    [SerializeField] [Range(0.01f, 10.0f)] private float stepFrequency;
    [SerializeField] private Transform Neck;

    private Vector3 lastPos;
    private Vector3 newPos;
    private float oldDist = 0;
    private float lerpVal = 0;
    private void Start()
    {
        lastPos = this.transform.position;
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
            newPos = new Vector3(getSin(amplitude / 2, stepFrequency, checkDist), getSin(amplitude, stepFrequency, checkDist), 0);
            Neck.localPosition = newPos;
        }
        else
        {
            Neck.localPosition = Vector3.zero;
            if (false) {
                Neck.localPosition = Vector3.Lerp(newPos, Vector3.zero, lerpVal);
                if (lerpVal < 1)
                {
                    lerpVal = lerpVal + 0.01f;
                }
                else
                {
                    Neck.position = Vector3.zero;
                }
            }
            
        }
        
    }

    private float getSin(float multiplier, float devisor,float x)
    {
        return multiplier * Mathf.Sin((x/3.14f) * 10 * devisor);
    }
    
}
