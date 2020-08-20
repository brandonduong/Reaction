using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableButton : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && target)
        {
            // Toggle all colliders
            Collider2D[] colliders = target.GetComponents<Collider2D>();

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = !colliders[i].enabled;
            }

            // Toggle sprite
            if (target.GetComponent<SpriteRenderer>() != null)
            {
                target.GetComponent<SpriteRenderer>().enabled = !target.GetComponent<SpriteRenderer>().enabled;
            }

            // Or toggle sprites
            else
            {
                SpriteRenderer[] sprites = target.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = !sprites[i].enabled;
                }
            }
        }
    }
}
