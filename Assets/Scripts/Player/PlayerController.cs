using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://youtu.be/PmIPqGqp8UY
// https://youtu.be/n-KX8AeGK7E?t=997

public class PlayerController : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform playerNeck = null;
    [SerializeField] private float mouseSensitivity = 4.0f;

    [SerializeField] private float neckStartAngle = 0f;
    [SerializeField] private float maxCameraAngle = 90f;
    [SerializeField] private float minCameraAngle = -90f;

    [SerializeField] private float neckLength = 0.2f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.001f;
    [SerializeField] private bool lockCursor = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField]private float speedUpVal = 0.2f;
    [SerializeField]private float speedDownVal = 0.1f;
    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.05f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] private float jumpHeight;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Animations")]
    [SerializeField]private Animator playerAnimator;
    private float animationVal = 0.0f;
    float f = 0.5f;

    public bool isGrounded;
    private float viewPitch = 0f;
    private float velocityY = 0.0f;
    private float speed;
    private CharacterController controller;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;


    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Position the camera at the height of the neck (set by the neckLength)
        neckLength = Vector3.Distance(playerCamera.position, playerNeck.position);
        playerCamera.position = playerNeck.position;
        playerCamera.position += playerNeck.up * neckLength;
    }
    private void Update()
    {
        f = (viewPitch + 90f) / 180f;

        playerAnimator.SetFloat("Time", 1-f);
        UpdateMouseLook();
        Grounded();
        UpdateMovement();
    }
    private void Grounded()
    {
        //Check every frame if the player stands on the ground
        Vector3 groundCheckPos = groundCheck.position;
        groundCheckPos += new Vector3(0,0,0);
        isGrounded = Physics.CheckSphere(groundCheckPos, groundDistance, groundMask);
    }
    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Get the axis of the mouse

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        viewPitch -= currentMouseDelta.y * mouseSensitivity;

        viewPitch = Mathf.Clamp(viewPitch,-maxCameraAngle,-minCameraAngle);

        if (viewPitch >= neckStartAngle)
        {
            playerNeck.localEulerAngles = Vector3.right * (viewPitch - neckStartAngle);
        }
        else
        {
            playerCamera.localEulerAngles = Vector3.right * viewPitch;
        }

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); //Rotate the hole player if looked sideways (Rotates the player left and right)
    }
    private void UpdateMovement()
    {
        //Grounded
        velocityY += gravity * Time.deltaTime;
        if (isGrounded && velocityY < 0)
            velocityY = 0.0f;

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Debug.Log("Jump");
            velocityY += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //Get Inputs
        targetDir.Normalize(); //Damit schräg laufen nicht schneller ist

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); //Smooth movement change

        

        if (Input.GetButton("Sprint") && isGrounded)
        { //If Sprint button is pressed the speed is switched form walking to sprinting
            if (speed <= sprintSpeed)
            {
                speed += sprintSpeed * speedUpVal;
            }
            else
            {
                speed = sprintSpeed;
            }
        }
        else
        {
            if (speed <= sprintSpeed)
            {
                speed += walkSpeed * speedDownVal;
            }
            else
            {
                speed = walkSpeed;
            }
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + (Vector3.up * velocityY);

        controller.Move(velocity * Time.deltaTime);

    }
  
}
