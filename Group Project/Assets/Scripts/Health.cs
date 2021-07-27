using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public GameObject healthHost;
    private string currentState;
    private Animator ani;

    //public Collider2D hitBox;


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
        if (currentHealth == 0)
        {
            ChangeAnimationState("Death");
            Invoke("Death", 1);
        }
    }

    public void Death()
    {
        
        Destroy(healthHost);
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
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log(healthHost + " took " + damage + " you health is :" + currentHealth);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Enemy")
    //    {
    //        Damage(1);
    //    }
    //}
}