using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Animations;

public class Enemy_AI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target1;
    public Transform target2;
    private Transform target;
    public float activeDistance = 10f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightRequirment = 0.8f;
    public float jumpModifier = .3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public bool isDying = false;
    [Header("Stats")]
    public int damage = 1;

    [Header("Animation")]
    public Transform enemyGFX;
    public Animator enemyAni;

    Path path;
    private int currentWayPoint = 0;
    private string currentState;
    Seeker seeker;
    Rigidbody2D rb;
    BoxCollider2D bc;
    CircleCollider2D attackZone;
    public LayerMask groundLayer;
    public LayerMask wallLayer;


    // Start is called before the first frame update
    void Start()
    {
        target = target1;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        attackZone = GetComponent<CircleCollider2D>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting the current animation
        if (currentState == newState) return;

        //play the animation
        enemyAni.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activeDistance;
    }

    private bool WallCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.right, 0.5f, wallLayer);
        return raycastHit.collider != null;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    void PathFollow()
    {
        ChangeAnimationState("Moving");

        if (path == null)
        {
            return;
        }

        //Reached end of path
        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }
        
        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = new Vector2(direction.x, 0f) * speed * Time.deltaTime;

        //Jump
        if (jumpEnabled && IsGrounded() && WallCheck())
        {
            if (direction.y > jumpNodeHeightRequirment)
            {
                ChangeAnimationState("Attacking");
                rb.MovePosition(Vector2.up * speed * jumpModifier);
            }
        }

        //Movement
        rb.MovePosition(rb.position + new Vector2(direction.x, 0f) * speed * Time.deltaTime);

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        //Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
        if (target == target1)
        {
            target = target2;
        }
        else
            target = target1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackZone.IsTouching(collision) && collision.CompareTag("Player") && !collision.isTrigger)
        {
            var healthComponent = collision.gameObject.GetComponent<Health>();
            var rbComponent = collision.gameObject.GetComponent<Rigidbody2D>();
            var playerController = collision.gameObject.GetComponent<Player_Mover>();
            if (healthComponent != null && healthComponent.canBeHurt == true)
            {
                rbComponent.velocity = Vector2.zero;
                rbComponent.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                StartCoroutine(KnockTime(rbComponent, healthComponent));
            }
        }
    }
    private IEnumerator KnockTime(Rigidbody2D player, Health healthComponent)
    {
        healthComponent.Damage(damage);
        healthComponent.canBeHurt = false;
        yield return new WaitForSeconds(0.5f);
        player.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        healthComponent.canBeHurt = true;
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
        else if (followEnabled == false)
        {
            if(isDying == true)
            {
                ChangeAnimationState("Death");
            }
            else
            {
                //ChangeAnimationState("Idle");
            }
        }
    }
}