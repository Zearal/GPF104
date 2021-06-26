using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Mover : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public BoxCollider2D bc;

    private float horizontal;
    private float speed = 3f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;
    private float slideFactor = 0.8f;
    private int jumpCount = 0;


    // Update is called once per frame
    void Update()
    {
        if (!IsOnWall())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.y) > 0f && !IsGrounded() && !IsOnWall())
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }

        IsSliding();

        if (IsGrounded())
        {
            jumpCount = 0;
        }

    }

    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
        
    }

    private bool IsOnWall()
    {
        if (isFacingRight)
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
            return raycastHit.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
            return raycastHit.collider != null;
        }
            //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }

    private void IsSliding()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (IsOnWall() && !IsGrounded())
        {
            Vector2 slide = rb.velocity;
            slide.y = -slideFactor;
            rb.velocity = slide;
            animator.SetBool("IsSliding", true);
            horizontal = 0;
            jumpCount = 0;
        }
        else
        {
            animator.SetBool("IsSliding", false);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
            horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsOnWall() && jumpCount < 1)
        {
            Flip();
            rb.velocity = new Vector2(jumpingPower, jumpingPower);
            jumpCount++;
        }

        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
}
