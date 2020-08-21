using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile : MonoBehaviour
{
    public GameObject breakEffect;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
    }
}
