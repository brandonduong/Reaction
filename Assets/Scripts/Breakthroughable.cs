using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakthroughable : MonoBehaviour
{
    public GameObject breakEffect;
    public GameObject player;
    private PlayerController2D controller;
    private Rigidbody2D playerRB;
    public float halfWidth;

    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
        controller = player.GetComponent<PlayerController2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Should break if recoiled into it, or fell into it from afar
        if (collision.gameObject.tag == "Player")
        {
            // Break from the sides
            if (controller.recentlyRecoiledBackwards)
            {
                Break();
            }

            else if (playerRB.position.x >= transform.position.x - halfWidth && playerRB.position.x <= transform.position.x + halfWidth)
            {
                // Break from top
                if (controller.recentlyRecoiledDownwards && playerRB.position.y >= transform.position.y)
                {
                    Break();
                }

                // Break from botton
                else if (controller.recentlyRecoiledUpwards && playerRB.position.y <= transform.position.y)
                {
                    Break();
                }
            }
        }
    }

    private void Break()
    {
        // Create effect + reference
        GameObject effect = (GameObject)Instantiate(breakEffect, player.GetComponent<Rigidbody2D>().position, transform.rotation);

        // Audio
        AudioManager.instance.PlaySound("BreakableBreak");

        // Destroy block
        Destroy(gameObject);

        // Destroy effect
        Destroy(effect, 1);
    }
}
