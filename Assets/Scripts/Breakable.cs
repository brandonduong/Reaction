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

        // Destroy breakable object
        Destroy(gameObject);

        // Destroy effect
        Destroy(effect, 1);
    }

}
