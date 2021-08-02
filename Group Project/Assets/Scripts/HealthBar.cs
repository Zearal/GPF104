using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Health health;

    private void Start()
    {
        GetHealth();
    }
    private void GetHealth()
    {
        GameObject player = GameObject.Find("Player");
        health = player.GetComponent<Health>();
    }
    // Update is called once per frame
    void Update()
    {
        if (health == null)
        {
            GetHealth();
        }
        slider.value = health.currentHealth;
    }
}
