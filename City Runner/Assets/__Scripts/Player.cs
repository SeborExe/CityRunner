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

    [Header("Checks Collisions")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator animator;

    private bool canRun;
    private bool isGrounded;
    private bool isRunning;
    private bool canDoubleJump;
    private bool isSliding;
    private bool canSlide;

    [SerializeField] private float slidingTime;
    [SerializeField] private float slidingCooldown;
    private float slidingTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.anyKey)
            canRun = true;

        CheckForRun();
        CheckForJump();
        CheckForSlide();

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
        if (isSliding)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canRun)
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

    private void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    private void CheckForCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
