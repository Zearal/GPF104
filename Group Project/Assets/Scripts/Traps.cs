using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    BoxCollider2D attackZone;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        attackZone = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
