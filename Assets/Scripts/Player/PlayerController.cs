using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// https://youtu.be/PmIPqGqp8UY
// https://youtu.be/n-KX8AeGK7E?t=997

public class PlayerController : NetworkBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform playerNeck = null;
    [SerializeField] private float mouseSensitivity = 4.0f;

    [SerializeField] private float maxCameraAngle = -90f;
    [SerializeField] private float neckStartAngle = 0f;
    [SerializeField] private float minCameraAngle = 90f;

    [SerializeField] private float neckLength = 0.2f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.001f;
    [SerializeField] private bool lockCursor = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.05f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] private float jumpHeight;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

<<<<<<< Updated upstream
=======
    [Header("Ground Angle")]
    [SerializeField] private float groundAngle;
    [SerializeField] private float moveGroundAngle;

    [Header("bullets per minute")]
    [SerializeField] private float bulletsPerMinute = 120f;
    [SerializeField] private bool canFire = true;



    [SerializeField] private GameObject gun;

>>>>>>> Stashed changes
    public bool isGrounded;
    private float fullPitch = 0f;
    private float cameraPitch = 0f;
    private float neckPitch = 0f;
    private float velocityY = 0.0f;
    private CharacterController controller;
    [SerializeField]private Animator anim;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;


    private void Start()
    {
        anim = gun.GetComponent<Animator>();
        if (isLocalPlayer)
        {
            controller = GetComponent<CharacterController>();
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }
        
    }
    private void Update()
    {
        if (!isLocalPlayer) return;

        UpdateMouseLook();
        Grounded();
        UpdateMovement();
        
    }
    private void Grounded()
    {
        //Check every frame if the player stands on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Get the axis of the mouse

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        fullPitch -= currentMouseDelta.y * mouseSensitivity;
        fullPitch = Mathf.Clamp(fullPitch,-maxCameraAngle,-minCameraAngle);

        if (fullPitch >= neckStartAngle) {
            playerNeck.localEulerAngles = Vector3.right * (fullPitch - neckStartAngle);
            
        }
        else {
            playerCamera.localEulerAngles = Vector3.right * fullPitch;


<<<<<<< Updated upstream
        }
        playerCamera.position = playerNeck.position;
        playerCamera.position += playerNeck.up * neckLength;
        
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); //Rotate the hole player if looked sideways (Rotates the player left and right)
    }
    private void UpdateMovement()
    {
=======
    IEnumerator OnGunReset()
    {
        //gun.GetComponent<Animator>().SetTrigger("shootTrigger");
        //gun.GetComponent<Animator>().Play("Shoot");
        anim.Play("Shoot");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).speed);
        //Debug.Log(anim.GetNextAnimatorStateInfo(0).speed);
        
        yield return new WaitForSeconds(0.5f/3f);

        canFire = true;
    }
    IEnumerator reset()
    {
        yield return new WaitForSeconds(60f / bulletsPerMinute);
        Debug.Log(60f / bulletsPerMinute);
        canFire = true;
    }


        private void UpdateMovement()
    {
        

        if (Input.GetButton("Fire1") && canFire)
        {
            
            canFire = false;
            anim.PlayInFixedTime("Base Layer.Shoot", 0, bulletsPerMinute);
            StartCoroutine("reset");
            //StartCoroutine("OnGunReset");
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).speed);
           //Debug.Log(anim.GetCurrentAnimatorClipInfo(0).Length);
            //Debug.Log("mult " + anim.GetNextAnimatorStateInfo(0).speedMultiplier);


        }
       // Debug.Log("2 " + canFire);
        if (Input.GetAxisRaw("Sprint") > 0 && isGrounded)
        {
            Debug.Log("Sprint");
            movementSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            movementSpeed = walkSpeed;
            isSprinting = false;
        }

>>>>>>> Stashed changes
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


        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + (Vector3.up * velocityY);

        controller.Move(velocity * Time.deltaTime);

    }
  
}
