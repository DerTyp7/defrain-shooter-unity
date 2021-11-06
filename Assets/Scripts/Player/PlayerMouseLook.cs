using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMouseLook : NetworkBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform playerNeck = null;
    [SerializeField] private float mouseSensitivity = 4.0f;

    [SerializeField] private float maxCameraAngle = 90f;
    [SerializeField] private float neckStartAngle = 0f;
    [SerializeField] private float minCameraAngle = -90f;

    private float neckLength = 0.2f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.001f;
    [SerializeField] private bool lockCursor = true;

    private float fullPitch = 0f;
    private float cameraPitch = 0f;
    private float neckPitch = 0f;
    private float velocityY = 0.0f;
    private CharacterController controller;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;



    private void Start()
    {
        if (isLocalPlayer)
        {
            controller = GetComponent<CharacterController>();

            playerCamera.gameObject.SetActive(true);
            neckLength = Vector3.Distance(playerNeck.position,playerCamera.position);

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            UpdateMouseLook();
        }

    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Get the axis of the mouse

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        fullPitch -= currentMouseDelta.y * mouseSensitivity;
        fullPitch = Mathf.Clamp(fullPitch, -maxCameraAngle, -minCameraAngle);

        if (fullPitch >= neckStartAngle)
        {
            playerNeck.localEulerAngles = Vector3.right * (fullPitch - neckStartAngle);

        }
        else
        {
            playerNeck.localEulerAngles = Vector3.right * 0f;
            playerCamera.localEulerAngles = Vector3.right * fullPitch;


        }
        playerCamera.position = playerNeck.position;
        playerCamera.position += playerNeck.up * neckLength;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); //Rotate the hole player if looked sideways (Rotates the player left and right)
    }

}
