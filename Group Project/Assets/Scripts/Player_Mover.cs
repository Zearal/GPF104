using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Mover : MonoBehaviour
{
    [Header("Attachments")]
    public Animator animator;
    public Animator dust;
    public AudioManager audio;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask enemyLayers;
    public BoxCollider2D bc;
    public CircleCollider2D weapon;


    private float horizontal;
    [Header("Stats")]
    public float speed = 6f;
    public float jumpingPower = 10f;
    private int attackDamage = 0;
    public bool isFacingRight = true;
    private bool isAttacking = false;
    public bool isDisabled = false;
    private float slideFactor = 1.5f;
    private int jumpCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (!IsOnWall() && !isAttacking)
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

        if (IsGrounded() && Mathf.Abs(rb.velocity.x) > 0f)
        {
            dust.SetBool("isRunning", true);
        }
        else
        {
            dust.SetBool("isRunning", false);
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
            RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.right, 0.1f, wallLayer);
            return raycastHit.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.left, 0.1f, wallLayer);
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
        if (!isDisabled)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isDisabled)
        {
            if (context.performed && IsOnWall() && jumpCount < 5 && !isAttacking)
            {
                audio.Play("Player_Jump");
                Flip();
                rb.velocity = new Vector2((rb.velocity.x * 1.25f), (jumpingPower * 1.25f));
                jumpCount++;
            }

            if (context.performed && IsGrounded() && !isAttacking)
            {
                audio.Play("Player_Jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                Debug.Log("jumping");
            }

            if (context.canceled && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsLightAttacking", true);
            isAttacking = true;
            attackDamage = 1;
            StartCoroutine(LightAttCooldown());
        }

    }
    IEnumerator LightAttCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsLightAttacking", false);
        attackDamage = 0;
        isAttacking = false;
    }
    public void Attack2(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsHeavyAttacking", true);
            isAttacking = true;
            attackDamage = 3;
            StartCoroutine(HeavyAttCooldown());
        }

    }
    IEnumerator HeavyAttCooldown()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsHeavyAttacking", false);
        attackDamage = 0;
        isAttacking = false;
    }
    public void Use(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("You used it");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if(weapon.IsTouching(collision) && collision.gameObject.tag == "Enemy")
        {
            var healthComponent = collision.gameObject.GetComponent<Health>();
            var rbComponent = collision.gameObject.GetComponent<Rigidbody2D>();
            if(healthComponent != null)
            {
                Vector2 difference = collision.transform.position - transform.position;
                difference = difference.normalized * 5;
                rbComponent.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockTime(rbComponent));
                healthComponent.Damage(attackDamage);
            }
        }
    }

    private IEnumerator KnockTime(Rigidbody2D enemy)
    {
        enemy.GetComponent<Enemy_AI>().followEnabled = false;
        yield return new WaitForSeconds(1f);
        enemy.GetComponent<Enemy_AI>().followEnabled = true;
    }
}