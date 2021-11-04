using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Headbob : NetworkBehaviour
{
    [SerializeField] private float posCheckDistance = 0.01f;
    [SerializeField] private float checkDist = 0.0f;

    [Header("Step Settings")]
    [SerializeField] private float stepAmplitude;
    [SerializeField] private float stepFrequency;
    [SerializeField] private Transform Neck;

    private Vector3 lastPos;
    private float oldDist = 0;
    private void Start()
    {
        lastPos = this.transform.position;
    }

    private void Update()
    {
        float dist = Vector3.Distance(lastPos, this.transform.position);
        
        
        if (dist > posCheckDistance)
        {
            checkDist += dist - oldDist;
            lastPos = this.transform.position;
            oldDist = dist;
        }
        if (checkDist > 2)
        {
            checkDist = 0;
        }
        Vector3 newPos = new Vector3(Neck.transform.position.x,getSin(stepAmplitude,stepAmplitude,checkDist),Neck.transform.position.z);
        Neck.position = newPos;
        Debug.Log("Distance: " + checkDist + ", Sin " + getSin(stepAmplitude, stepAmplitude, checkDist));
        
    }

    private float getSin(float multiplier, float devisor,float x)
    {
        return multiplier * Mathf.Sin((x/Mathf.PI)*10);
    }
    
}
