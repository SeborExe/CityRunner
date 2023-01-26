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
    [SerializeField] float minVelocityToRoll;

    [Header("Checks Collisions")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Transform bottomWallCheckTransform;
    [SerializeField] Transform ledgeCheckTransform;
    [SerializeField] Transform wallCheckTransform;
    [SerializeField] Transform ceilingCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator animator;

    private bool canRun;
    private bool isGrounded;
    private bool isBottomWallDetected;
    private bool isRunning;
    private bool canDoubleJump;
    private bool isSliding;
    private bool canSlide;
    [SerializeField] private bool canRoll = false;
    private bool isTouchingLedge;
    private bool isWallDetected;
    private bool isLedgeDetected;
    private bool canClimbLedge;
    private bool isCeilingDetected;

    [SerializeField] private float slidingTime;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedIncreaseMilestone;

    private float slidingTimer;
    private float speedMilestone;
    private float defaultMoveSpeed;
    private float defaultMilestoneSpeed;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1; // position to hold player before animation end
    private Vector2 ledgePos2; // position where to move player after animation end
    [SerializeField] float ledgeClimbe_Xoffset1 = 0f;
    [SerializeField] float ledgeClimbe_Xoffset2 = 0f;
    [SerializeField] float ledgeClimbe_Yoffset1 = 0f;
    [SerializeField] float ledgeClimbe_Yoffset2 = 0f;

    [Header("Coin Info"), Space(15)]
    public int coins;

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
        CheckForLedgeClimb();

        HandleAnimations();
        CheckForCollisions();
    }

    private void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSliding", isSliding);
        animator.SetBool("CanClimbeLedge", canClimbLedge);
        animator.SetBool("CanDoubleJump", canDoubleJump);
        animator.SetBool("CanRoll", canRoll);


        if (canClimbLedge)
        {
            canRoll = false;
        }

        else if (rb.velocity.y < minVelocityToRoll)
        {
            canRoll = true;
        }
    }

    private void CheckForRun()
    {
        if (canRun)
        {
            if (isBottomWallDetected || isWallDetected && !isSliding)
            {
                SpeedReset();
            }

            else if (isSliding)
            {
                rb.velocity = new Vector2(moveSpeed * slideSpeedMultiplier, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
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
                canRoll = false;
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide && rb.velocity.x > defaultMoveSpeed)
        {
            isSliding = true;
            canSlide = false;
            slidingTimer = Time.time;
        }

        if (Time.time > slidingTimer + slidingTime && !isCeilingDetected)
        {
            isSliding = false;
        }

        if (Time.time > slidingTimer + slidingCooldown)
        {
            canSlide = true;
        }
    }

    private void CheckForLedgeClimb()
    {
        if (isLedgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            ledgePos1 = new Vector2((ledgePosBot.x + wallCheckDistance) + ledgeClimbe_Xoffset1, (ledgePosBot.y) + ledgeClimbe_Yoffset1);
            ledgePos2 = new Vector2(ledgePosBot.x + wallCheckDistance + ledgeClimbe_Xoffset2, (ledgePosBot.y) + ledgeClimbe_Yoffset2);

            canRun = false;
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    private void CheckIfLedgeClimbeFinished()
    {
        transform.position = ledgePos2;
        canClimbLedge = false;
        canRun = true;
        isLedgeDetected = false;
    }

    private void RollAnimationFinished()
    {
        canRoll = false;
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
        isBottomWallDetected = Physics2D.Raycast(bottomWallCheckTransform.position, Vector2.right, wallCheckDistance, whatIsGround);
        isCeilingDetected = Physics2D.Raycast(ceilingCheckTransform.position, Vector2.up, wallCheckDistance + 0.5f, whatIsGround); ;

        isTouchingLedge = Physics2D.Raycast(ledgeCheckTransform.position, Vector2.right, wallCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheckTransform.position, Vector2.right, wallCheckDistance, whatIsGround);

        if (isWallDetected && !isTouchingLedge && !isLedgeDetected)
        {
            isLedgeDetected = true;
            ledgePosBot = wallCheckTransform.position;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawLine(bottomWallCheckTransform.position, new Vector3(bottomWallCheckTransform.position.x + wallCheckDistance, bottomWallCheckTransform.position.y, 
            bottomWallCheckTransform.position.z));

        Gizmos.DrawLine(wallCheckTransform.position, new Vector3(wallCheckTransform.position.x + wallCheckDistance, wallCheckTransform.position.y,
            wallCheckTransform.position.z));

        Gizmos.DrawLine(ledgeCheckTransform.position, new Vector3(ledgeCheckTransform.position.x + wallCheckDistance, ledgeCheckTransform.position.y,
             ledgeCheckTransform.position.z));

        Gizmos.DrawLine(ceilingCheckTransform.position, new Vector3(ceilingCheckTransform.position.x, ceilingCheckTransform.position.y + wallCheckDistance,
            ceilingCheckTransform.position.z));
    }
}
