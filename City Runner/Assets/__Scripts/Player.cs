using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float doubleJumpForce;
    [SerializeField] float slideSpeedMultiplier;
    [SerializeField] float maxMoveSpeed;

    [Header("Checks Collisions")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Transform wallCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator animator;

    private bool canRun;
    private bool isGrounded;
    private bool isWallDetected;
    private bool isRunning;
    private bool canDoubleJump;
    private bool isSliding;
    private bool canSlide;

    [SerializeField] private float slidingTime;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedIncreaseMilestone;

    private float slidingTimer;
    private float speedMilestone;
    private float defaultMoveSpeed;
    private float defaultMilestoneSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetDefaltueVelues();
    }

    private void Update()
    {
        if (Input.anyKey)
            canRun = true;

        CheckForRun();
        CheckForJump();
        CheckForSlide();
        CheckForSpeedingUp();

        HandleAnimations();
        CheckForCollisions();
    }

    private void HandleAnimations()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSliding", isSliding);
    }

    private void CheckForRun()
    {
        if (isWallDetected)
        {
            SpeedReset();
        }

        else if (isSliding)
        {
            rb.velocity = new Vector2(moveSpeed * slideSpeedMultiplier, rb.velocity.y);
        }

        else if (canRun)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        if (rb.velocity.x > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void CheckForSpeedingUp()
    {
        if (transform.position.x > speedMilestone)
        {
            speedMilestone += speedIncreaseMilestone;
            moveSpeed *= speedMultiplier;
            speedIncreaseMilestone *= speedMultiplier;

            if (moveSpeed > maxMoveSpeed)
                moveSpeed = maxMoveSpeed;
        }
    }

    private void CheckForJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump(jumpForce);
            }
            else if (canDoubleJump)
            {
                Jump(doubleJumpForce);
                canDoubleJump = false;
            }
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }

    private void CheckForSlide()
    {
        if (!isGrounded) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide)
        {
            isSliding = true;
            canSlide = false;
            slidingTimer = Time.time;
        }

        if (Time.time > slidingTimer + slidingTime)
        {
            isSliding = false;
        }

        if (Time.time > slidingTimer + slidingCooldown)
        {
            canSlide = true;
        }
    }

    private void SetDefaltueVelues()
    {
        defaultMoveSpeed = moveSpeed;
        defaultMilestoneSpeed = speedIncreaseMilestone;
        speedMilestone = speedIncreaseMilestone;
    }

    private void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    private void SpeedReset()
    {
        moveSpeed = defaultMoveSpeed;
        speedIncreaseMilestone = defaultMilestoneSpeed;
    }

    private void CheckForCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheckTransform.position, Vector2.right, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheckTransform.position, new Vector3(wallCheckTransform.position.x + wallCheckDistance, wallCheckTransform.position.y, 
            wallCheckTransform.position.z));
    }
}
