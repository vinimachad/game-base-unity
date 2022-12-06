using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class MoveBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private float groundDrag = 5f;

    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float airMultiplier = .4f;
    [SerializeField] private bool readyToJump = true;

    public Vector2 movInput;

    [SerializeField] private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("Keybinds")]

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool grounded;

    [SerializeField] private Transform orientation;
    [SerializeField] private Animator animator;

    [Header("Animation")]
    private string isSprintingParam = "IsSprinting";
    private string isRunningParam = "IsRunning";
    private string isJumpingParam = "IsJumping";


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public virtual void FixedUpdate()
    {
        MovePlayer(movSpeed);

        if (Input.GetKey(sprintKey) && IsMoving())
        {
            SprintPlayer();
            animator.SetBool(isSprintingParam, true);
        }
        else
        {
            animator.SetBool(isSprintingParam, false);
        }
    }

    public virtual void Update()
    {
        MyInput();
        SpeedControl();
        GroundCheck();
    }

    public virtual void MyInput()
    {
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
        }
    }

    private void MovePlayer(float velocity)
    {
        moveDirection = orientation.forward * movInput.y + orientation.right * movInput.x;

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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        animator.SetBool(isJumpingParam, false);
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, .1f, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down * .1f, Color.red);

        if (grounded)
        {
            rb.drag = groundDrag;
            ResetJump();
        }
        else
        {
            animator.SetBool(isJumpingParam, true);
            rb.drag = 0;
        }
    }

    private bool IsMoving()
    {
        return moveDirection.magnitude != 0;
    }
}
