using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// https://youtu.be/PmIPqGqp8UY
// https://youtu.be/n-KX8AeGK7E?t=997

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private ProcedualAnimationController procedualAnimationController;
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 7.0f;
    [SerializeField] private float aimWalkSpeed = 3.0f;
    [SerializeField] private float fallDamageSpeed = 10.0f;
    

    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.001f;
    [SerializeField] float gravity = -10.0f;
    [SerializeField] private float jumpHeight;
    public Vector3 inputDirection = Vector3.zero;
    private Vector3 moveDirection;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    [Header("Ground Angle")]
    [SerializeField] private float groundAngle;
    [SerializeField] private float moveGroundAngle;

    public bool isGrounded;
    public bool isSprinting;
    public float currentMaxSpeed = 5.0f;
    private float velocityY = 0.0f;
    private CharacterController controller;

    public Vector3 currentDir = Vector3.zero;
    private Vector3 currentDirVelocity = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 localVelocity = Vector3.zero;
    private Vector3 refVelocity = Vector3.zero;


    private void Start()
    {
        if (isLocalPlayer)
        {
            controller = GetComponent<CharacterController>();
        }
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            Grounded();
            CheckGoundAngle();
            UpdateMovement();
        }
        
    }
    private void Grounded()
    {
        //Check every frame if the player stands on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position + new Vector3(0, GetComponent<CharacterController>().radius - 0.1f, 0),GetComponent<CharacterController>().radius + 0.0f, groundMask);
    }

    public bool isMoving() 
    {
        if (velocity.x == 0 && velocity.y == 0 && velocity.z == 0) return false;
        else return true;
    }

    private void CheckGoundAngle()
    {
        //Check every frame if the player stands on the ground
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position + new Vector3(0, 0.4f, 0), Vector3.down, out hit))
        {
            moveDirection = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0) * inputDirection;
            moveDirection = Vector3.Cross(moveDirection, hit.normal);

            if (isMoving())
            {
                moveGroundAngle = -(Vector3.Angle(moveDirection, transform.up) - 90f);
            }
            else
            {
                moveGroundAngle = 0f;
            }

            groundAngle = Vector3.Angle(hit.normal,transform.up);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(transform.position, moveDirection * 50));
    }
    [Command]
    void CmdFallDamage(int damage) 
    {
        GetComponent<Player>().RemoveHealth(damage);
    }
    private void UpdateMovement()
    {


        if (Input.GetAxisRaw("Sprint") > 0 && isGrounded && !procedualAnimationController.isAiming)
            {
            currentMaxSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {

            if(procedualAnimationController.isAiming) currentMaxSpeed = aimWalkSpeed;
            else currentMaxSpeed = walkSpeed;
            isSprinting = false;

        }

        if(isGrounded && velocity.y < -fallDamageSpeed)
        {
            CmdFallDamage((int)Mathf.Abs(velocity.y));
        }

        //Grounded
        if (velocityY < 0)
        {
            velocityY += gravity * 0.9f * Time.deltaTime;
        }
        else
        {
            velocityY += gravity* Time.deltaTime;
        }
        if (isGrounded && velocityY < 0)
            velocityY = 0.0f;

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Debug.Log("Jump");
            velocityY += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")); //Get Inputs
        
        inputDirection.Normalize(); //Damit schräg laufen nicht schneller ist

        if (isGrounded)
        {
            currentDir = moveDirection;
        }
        else
        {
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); 
            currentDir = moveDirection;
        }
        velocity = Vector3.SmoothDamp(velocity, currentDir * currentMaxSpeed + new Vector3(0, velocityY, 0),ref refVelocity,moveSmoothTime);
        localVelocity = transform.InverseTransformDirection(velocity);
        controller.Move(velocity * Time.deltaTime);
        //transform.position += velocity * Time.deltaTime;
    }
  
}
