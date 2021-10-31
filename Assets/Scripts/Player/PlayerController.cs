using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// https://youtu.be/PmIPqGqp8UY
// https://youtu.be/n-KX8AeGK7E?t=997

public class PlayerController : NetworkBehaviour
{

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.05f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] private float jumpHeight;
    private Vector3 inputDirection = Vector3.zero;
    private Vector3 moveDirection;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    [Header("Ground Angle")]
   // [SerializeField] private Transform groundDistance;
   

    public bool isGrounded;
    private float velocityY = 0.0f;
    private CharacterController controller;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;


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
        isGrounded = Physics.CheckSphere(groundCheck.position + new Vector3(0, GetComponent<CharacterController>().radius - 0.01f, 0),GetComponent<CharacterController>().radius + 0.0f, groundMask);
    }

    private void CheckGoundAngle()
    {
        //Check every frame if the player stands on the ground
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position + new Vector3(0,0.4f,0),Vector3.down,out hit)) 
        {
            Debug.Log(transform.eulerAngles.y);
            moveDirection =  Quaternion.Euler(0, transform.eulerAngles.y + 90, 0) * inputDirection;
            moveDirection = Vector3.Cross(moveDirection, hit.normal);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(transform.position, moveDirection * 50));
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

        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")); //Get Inputs
        inputDirection.Normalize(); //Damit schräg laufen nicht schneller ist

        currentDir = Vector2.SmoothDamp(currentDir, new Vector2(inputDirection.x,inputDirection.z), ref currentDirVelocity, moveSmoothTime); //Smooth movement change


        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + (Vector3.up * velocityY);

        controller.Move(velocity * Time.deltaTime);

    }
  
}
