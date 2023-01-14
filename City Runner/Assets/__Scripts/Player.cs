using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Checks")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask whatIsGround;

    private Rigidbody2D rb;

    private bool canRun;
    private bool grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.anyKey)
            canRun = true;

        CheckForRun();
        CheckForJump();
    }

    private void CheckForRun()
    {
        if (canRun)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    private void CheckForJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, whatIsGround);

            if (grounded)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
