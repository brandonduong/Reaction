using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int health = 100;

    public GameObject breakEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Break();
        }
    }

    void Break()
    {
        // Create effect + reference
        GameObject effect = (GameObject)Instantiate(breakEffect, transform.position, transform.rotation);

        // Audio
        AudioManager.instance.PlaySound("BreakableBreak");

        // Destroy breakable object
        Destroy(gameObject);

        // Destroy effect
        Destroy(effect, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("hit");
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }
    }
}
