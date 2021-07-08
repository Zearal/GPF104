using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Mover : MonoBehaviour
{
    private Core core;
    [Header("Attachments")]
    public Animator animator;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public BoxCollider2D bc;


    private float horizontal;
    [Header("Stats")]
    public float speed = 6f;
    public float jumpingPower = 10f;
    private bool isFacingRight = true;
    private float slideFactor = 1.5f;
    private int jumpCount = 0;

    private void Awake()
    {
        
    }
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
    }

    private void IsSliding()
    {
        if (IsOnWall() && !IsGrounded())
        {
            Vector2 slide = rb.velocity;
            slide.y = -slideFactor;
            rb.velocity = slide;
            animator.SetBool("IsSliding", true);
            horizontal = 0;
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
        if (context.performed && IsOnWall() && jumpCount < 3)
        {
            Flip();
            rb.velocity = new Vector2((rb.velocity.x * 1.25f), (jumpingPower * 1.25f));
            jumpCount++;
        }

        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("jumping");
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Attacking");
            core.Damage(3);
            core.Heal(4);
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("You used it");
            
        }
    }
}
