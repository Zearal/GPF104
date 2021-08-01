using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public bool canBeHurt = true;
    public bool isDead = false;
    
    public GameObject healthHost;
    private string currentState;
    private Animator ani;
    public int score = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ani = GetComponent<Animator>();
        if (ani == null)
        {
            ani = GetComponentInChildren<Animator>();
        }
    }
    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting the current animation
        if (currentState == newState) return;

        //play the animation
        ani.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 0 && !isDead)
        {
            ChangeAnimationState("Death");
            if (healthHost.CompareTag("Enemy"))
            {
                CircleCollider2D attackZone = healthHost.GetComponent<CircleCollider2D>();
                healthHost.GetComponent<Enemy_AI>().followEnabled = false;
                healthHost.GetComponent<Enemy_AI>().isDying = true;
                attackZone.enabled = false;
            }
            GameManager.instance.AddScore(score);
            Debug.Log("Added score");
            BoxCollider2D hitbox = healthHost.GetComponent<BoxCollider2D>();
            hitbox.enabled = false;
            isDead = true;
            Invoke("Death", 2);
        }
    }

    public void Death()
    {
        if (healthHost.CompareTag("Player"))
        {
            Debug.Log("GAME OVER");  
        }
        else
        {
            Destroy(healthHost);
        }
    }

    public void Heal(int heal)
    {
        while (heal != 0)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth++;
            }
            heal--;
        }
        Debug.Log("You Healed your health is :" + currentHealth);
    }

    public void Damage(int damage)
    {
        if (canBeHurt)
        {           
            currentHealth -= damage;
            StartCoroutine(Invurable());
            if (currentHealth < 0)
            {
                currentHealth = 0;
                Debug.Log("Target is Dead");
            }
            else
            {
                Debug.Log(healthHost + " took " + damage + " you health is :" + currentHealth);
            }
        }
        else
        {
            Debug.Log("Cant be hurt");
        }
    }
    IEnumerator Invurable()
    {
        canBeHurt = false;
        ChangeAnimationState("Hurt");
        yield return new WaitForSeconds(1f);
        canBeHurt = true;
        if (currentHealth != 0)
        {
            ChangeAnimationState("Idle");
        }
    }
}