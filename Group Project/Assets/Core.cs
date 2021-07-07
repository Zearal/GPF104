using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Core : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        Debug.Log("You Healed " + heal + " you health is :" + currentHealth);
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("You took " + damage + " you health is :" + currentHealth);
    }
}
