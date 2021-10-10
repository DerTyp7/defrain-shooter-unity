using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://youtu.be/PmIPqGqp8UY
// https://youtu.be/n-KX8AeGK7E?t=997

public class PlayerController : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity = 4.0f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.001f;
    [SerializeField] private bool lockCursor = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.05f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpMultiplier;


    private bool isJumping;

    private float cameraPitch = 0f;
    private float velocityY = 0.0f;
    private CharacterController controller;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        JumpInput();
    }
    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Get the axis of the mouse

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity; //minus, weil der rotation wert inverted ist
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); //Rotate the hole player if looked sideways
    }
    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //Get Inputs
        targetDir.Normalize(); //Damit schräg laufen nicht schneller ist

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); //Smooth movement change

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

    }
    private void JumpInput()
    {
        Debug.Log(controller.isGrounded);
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            isJumping = true;
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
        }
    }

  
}
