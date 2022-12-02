using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float sprintSpeed;
    public float movSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Keybinds")]

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public Animator animator;

    [Header("Animation")]
    private string isSprintingParam = "IsSprinting";
    private string isRunningParam = "IsRunning";
    private string isJumpingParam = "IsJumping";


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer(movSpeed);

        if (Input.GetKey(sprintKey) && IsMoving())
        {
            SprintPlayer();
            animator.SetBool(isSprintingParam, true);
        } else
        {
            animator.SetBool(isSprintingParam, false);
        }
    }

    void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, .5f, whatIsGround);

        MyInput();
        SpeedControl();   
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
        }
    }

    private void MovePlayer(float velocity)
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * velocity * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * velocity * 10f * airMultiplier, ForceMode.Force);
        }

        animator.SetBool(isRunningParam, IsMoving());
    }

    private void SprintPlayer()
    {
        MovePlayer(sprintSpeed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > movSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        animator.SetBool(isJumpingParam, true);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        animator.SetBool(isJumpingParam, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = collision.gameObject.CompareTag("Ground");

        if (grounded)
        {
            rb.drag = groundDrag;
            ResetJump();
        } else
        {
            rb.drag = 0;
        }
    }

    private bool IsMoving()
    {
        return moveDirection.magnitude != 0;
    }
}