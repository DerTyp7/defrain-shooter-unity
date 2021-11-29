using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProcedualAnimationController : NetworkBehaviour
{
    [Header("Hand Settings")]
    [SerializeField] private Transform rightHandREF;
    [SerializeField] private Transform leftHandREF;
    [SerializeField] private Transform gunRightHandREF;
    [SerializeField] private Transform gunLeftHandREF;
    [SerializeField] private Vector3   defaultRightHandPosition = Vector3.zero;
    [SerializeField] private Vector3   defaultLeftHandPosition  = Vector3.zero;


    [Header("Step Settings")]
    [SerializeField] private float stepAmplitudeWalking;
    [SerializeField] private float stepAmplitudeSprinting;
    [SerializeField] private float stepFrequency;
    private float checkDist = 0f;
    private float stepDistance = 1;
    private float stepSin;
    float moveSoftStart = 0;
    Vector3 lastPos = Vector3.zero;
    float posCheckDistance = 0.01f;
    float currentDist = 0;


    float f = 0f;

    [SerializeField] private PlayerController playerController;




    [Header("GameObjects")]
    [SerializeField] private float currentCameraRecoilX = 0f;
    [SerializeField] private float currentCameraRecoilY = 0f;
    [Header("GameObjects")]
    [SerializeField] private GameObject gunHolder;
    [SerializeField] private WeaponManager weaponManager;


    [Header("General Settings")]
    [SerializeField] bool positionRecoil = true;
    [SerializeField] bool rotationRecoil = true;

    [Header("Position Settings")]
    [SerializeField] float positionMultX = 25f;
    [SerializeField] float positionMultY = 25f;
    [SerializeField] float positionMultZ = 25f;

    [Header("Rotation Settings")]
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] float cameraRecoilX = 0.1f;
    [SerializeField] float cameraRecoilY = 0.1f;


    [SerializeField] bool rotX = true;
    [SerializeField] float rotationMultX = 25f;
    [SerializeField] float rotationOffsetX = 0.1f;
    [SerializeField] bool rotY = true;
    [SerializeField] float rotationMultY = 25f;
    [SerializeField] bool rotZ = true;
    [SerializeField] float rotationMultZ = 15f;

    [Header("Swey Settings")]
    [SerializeField] bool sideSwey = true;
    [SerializeField] float sweyMultX = 15f;
    [SerializeField] float sweyMultY = 15f;
    [SerializeField] float sweyMultZ = 15f;
    [SerializeField] float sweyWhileAim = 0.1f;
    float swey = 0f;

    [Header("External Settings")]
    [SerializeField] const int externalPositionVectorsNum = 1;
    private Vector3[] externalPositionVectors = new Vector3[externalPositionVectorsNum];


    [SerializeField] float returnForce = 0.006f;
    [SerializeField] float impulsForce = 0.025f;
    [SerializeField] float maxRecoil = 0.1f;

    private Animator gunAnimator;
    [SerializeField] private Animator playerAnimator;

    Vector3 startPos, startRot;
    float recoilOffset = 0f;
    float zOffset = 0f;
    float zVelocity = 0f;

    int recoilCounter = 0;

    [Header("Aiming Settings")]
    [SerializeField] float aimSpeed = 0.01f;
    [Range(0, 1)] public float aimVal = 0;
    [SerializeField] GameObject AimPoint;
    [SerializeField] GameObject HoldPoint;
    public bool isAiming = false;

    Vector3[] positionMod = new Vector3[3];
    public Quaternion[] rotationMod = new Quaternion[3];

    public void walkAnimation() 
    {
        playerAnimator.SetFloat("x", playerController.localVelocity.x / playerController.currentMaxSpeed);
        playerAnimator.SetFloat("y", playerController.localVelocity.z / playerController.currentMaxSpeed);
    }

    public void handPositioning()
    {
        if (gunRightHandREF != null)
        {
            rightHandREF.position = gunRightHandREF.position;
            rightHandREF.rotation = gunRightHandREF.rotation;
        }
        else 
        {
            rightHandREF.position = defaultRightHandPosition;
        }
        if (gunLeftHandREF != null)
        {
            leftHandREF.position = gunLeftHandREF.position;
            leftHandREF.rotation = gunLeftHandREF.rotation;
        }
        else
        {
            leftHandREF.position = defaultLeftHandPosition;
        }
    }
    public void OnSwitchWeapon(GameObject currentWeapon)
    {
        if (isLocalPlayer) {
            if(currentWeapon != null) {
                gunHolder = currentWeapon;
                gunAnimator = currentWeapon.GetComponent<Weapon>().WeaponAnimator;
                gunAnimator.SetFloat("ShootSpeed", 1f / (60f / currentWeapon.GetComponent<Weapon>().Firerate));
            }
        }
    }

    public void Recoil(float force)
    {
        if (isLocalPlayer) 
        { 
            //Play the animation
            gunAnimator.Play("Shoot");
            //Add force for the recoil
            //currentCameraRecoilX -= weightedPerlinNoise(cameraRecoilX, 1f, Time.time, 1);
            //playerMouseLook = Mathf.SmoothDamp(playerMouseLook.fullPitch, playerMouseLook.fullPitch - weightedPerlinNoise(cameraRecoilX, 1f, Time.time, 1),ref f,0.01f);
            CmdRecoil();
        }
    }
    [Command]
    private void CmdRecoil()
    {
        recoilCounter++;
        playerMouseLook.fullPitch -= weightedPerlinNoise(cameraRecoilX, 1f, Time.time, 1);
        transform.Rotate(Vector3.up * weightedPerlinNoise(cameraRecoilY, 1f, Time.time, 1f, -0.5f));
    }
    [Command]
    private void CmdAim(bool input) 
    {
        if (input) isAiming = true;
        else isAiming = false;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            walkAnimation();
            handPositioning();
            CmdAim(Input.GetButton("Aim"));
        }
        /*-----Aiming-----*/
        
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            positionMod = new Vector3[3];
            rotationMod = new Quaternion[3];
            /*-----Recoil-----*/
            calcRecoilOffset();
            /*-----Position Recoil-----*/
            PositionRecoil();
            /*-----Rotation Recoil-----*/
            RotationRecoil();
            /*-----Step Swey-----*/
            calcStepSin();
            SideStep();
            /*-----Aiming-----*/
            AimDownSights();
            /*-----Apply Gun Position-----*/
            Vector3 totalPosition = Vector3.zero;
            for (int i = 0; i < positionMod.Length; i++)
            {
                if (positionMod[i] != null)
                    totalPosition += positionMod[i];
            }

            /*-----Apply Gun Rotation-----*/
            Quaternion totalRotation = Quaternion.identity;
            for (int i = 0; i < rotationMod.Length; i++)
            {
                totalRotation *= rotationMod[i];
            }
            //gunHolder.transform.localPosition = positionMod[2];
            gunHolder.transform.localPosition = totalPosition;
            //gunHolder.transform.localRotation = totalRotation;
            gunHolder.transform.localRotation = rotationMod[0];
        }
    }

    void RotationRecoil() 
    {
        /*-----Rotation Recoil-----*/
        if (rotationRecoil)
        {
            float deltaX = 100f * rotationMultX * weightedPerlinNoise(recoilOffset, 0.5f, Time.time, 1f);
            float deltaY = 100f * rotationMultY * weightedPerlinNoise(recoilOffset, 0.5f, Time.time, 1f, -0.5f);
            float deltaZ = 100f * rotationMultZ * weightedPerlinNoise(recoilOffset, 0.5f, Time.time, 1f, -0.5f);

            rotationMod[0] = Quaternion.Euler(deltaX, deltaY, deltaZ);
        }
    }
    void PositionRecoil()
    {
        if (positionRecoil)
        {
            float deltaX = -positionMultX * weightedPerlinNoise(recoilOffset, 0.7f, Time.time, 1f, -0.5f);
            float deltaY = -positionMultY * weightedPerlinNoise(recoilOffset, 0.5f, Time.time, 2f);
            float deltaZ = positionMultZ * weightedPerlinNoise(recoilOffset, 0.5f, Time.time, 3f);

            positionMod[0] = new Vector3(deltaX, deltaY, deltaZ);
        }
    }
    void AimDownSights() 
    {
        aimVal = gravityValue(aimVal, aimSpeed, 1, 0, isAiming);
        positionMod[2] = Vector3.Lerp(HoldPoint.transform.localPosition, AimPoint.transform.localPosition, Mathf.Pow(aimVal, 1.3f));
    }
    void calcRecoilOffset() 
    {
        for (int i = 0; i < recoilCounter; i++)
        {
            zVelocity -= weightedPerlinNoise(impulsForce, 0.1f, i, 1f);
        }
        recoilCounter = 0;

        recoilOffset += zVelocity;

        zVelocity = 0;

        if (recoilOffset > 0)
            recoilOffset = 0f;
        else if (recoilOffset < 0)
            zVelocity += weightedPerlinNoise(returnForce, 0.1f, Time.time, 1f);

        recoilOffset = Mathf.Clamp(recoilOffset, -weightedPerlinNoise(maxRecoil, 0.5f, Time.time * 1000, 1f), 0);

    }
    void calcStepSin() 
    {
        float amplitude;

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
        stepSin = ezSin(stepAmplitudeWalking, stepFrequency, currentDist);
    }
    void SideStep() 
    {
        if (sideSwey)
        {
            //To start and end the sweying motion softly

            moveSoftStart = gravityValue(moveSoftStart, 0.1f, 0.03f, 1f, 0f, playerController.isMoving() && playerController.isGrounded);

            if (playerController.isMoving())
            {
                float deltaX = sweyMultX * moveSoftStart * Mathf.Clamp(Vector3.Magnitude(playerController.velocity), 0, 1) * weightedPerlinNoise(stepSin, 0.3f, Time.time, 10f, 0.5f) * Mathf.Clamp((1 - aimVal) * (1 - aimVal), sweyWhileAim, 1f);
                float deltaY = sweyMultY * moveSoftStart * Mathf.Clamp(Vector3.Magnitude(playerController.velocity), 0, 1) * weightedPerlinNoise(stepSin, 0.7f, Time.time, 20f, -0.5f) * Mathf.Clamp((1 - aimVal) * (1 - aimVal), sweyWhileAim, 1f);
                float deltaZ = sweyMultZ * moveSoftStart * Mathf.Clamp(Vector3.Magnitude(playerController.velocity), 0, 1) * weightedPerlinNoise(stepSin, 0.3f, Time.time, 30f, 0.5f) * Mathf.Clamp((1 - aimVal) * (1 - aimVal), sweyWhileAim, 1f);

                positionMod[1] = new Vector3(deltaX, deltaY, deltaZ);
            }
        }
    }




    /*-----Helper Methods-----*/
    float gravityValue(float curretnValue, float rateOfChange, float maxValue, float minValue, bool add)
    {
        // The currentValue will be advanced or reduced by the rateOfChange depending on the add boolean. But only in the specified range.
        // Usage: val = gravityValue(val, 0.01f, 1, 0, true);
        float value = curretnValue;
        if (add) value += rateOfChange;
        else value -= rateOfChange;

        return Mathf.Clamp(value, minValue, maxValue);
    }
    float weightedPerlinNoise(float value, float weight, float pX, float pY, float offset = 0f)
    {
        return value * (1f - weight) + value * weight * (Mathf.PerlinNoise(pX, pY) + offset);
    }

    float gravityValue(float curretnValue, float rateOfChangePos, float rateOfChangeNeg, float maxValue, float minValue, bool add)
    {
        // The currentValue will be advanced by the rateOfChangePos and reduced by the rateOfChangeNeg depending on the add boolean. But only in the specified range.
        // Usage: val = gravityValue(val, 0.01f, 0.05f, 1, 0, true);
        float value = curretnValue;
        if (add) value += rateOfChangePos;
        else value -= rateOfChangeNeg;

        return Mathf.Clamp(value, minValue, maxValue);
    }

    private float ezSin(float amplitude, float frequency, float x)
    {
        // Simplification of the sin function.
        return amplitude * Mathf.Sin((x / 3.1831f) * 10 * frequency);
    }
}
