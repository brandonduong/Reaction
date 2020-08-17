using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public GameObject player;
    private PlayerController2D controller;
    public float velocityMultiplier = 10f;
    public float velocityReducer = 11.5f;

    private void Start()
    {
        controller = player.GetComponent<PlayerController2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tag is untagged if trampoline should be considered as "ground"
        if (gameObject.tag == "Untagged")
        {
            gameObject.tag = "Trampoline";
        }

        if (gameObject.tag == "Trampoline")
        {
            // Handles y axis behaviour
            if (collision.relativeVelocity.y < 0f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * velocityMultiplier * (-(collision.relativeVelocity.y + velocityReducer)));
            }
            else if (collision.relativeVelocity.y > 0f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * velocityMultiplier * (collision.relativeVelocity.y - velocityReducer));
            }

            // Special case if player is using the trampoline
            if (collision.gameObject.tag == "Player")
            {
                controller.onTrampoline = true;

                // If velocity is insignificant, consider trampoline as "ground"
                if (Mathf.Abs(collision.relativeVelocity.y) < 15f)
                {
                    gameObject.tag = "Untagged";
                    controller.onTrampoline = false;
                }
            }
        }
    }
}
