using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public GameObject rune;
    public int score = 100;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.Play("Rune");
        GameManager.instance.AddScore(score);
        Destroy(rune);
    }
}
