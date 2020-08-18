using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject breakEffect;
    public GameObject player;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Create effect + reference
            GameObject effect = (GameObject)Instantiate(breakEffect, player.GetComponent<Rigidbody2D>().position, transform.rotation);

            // Destroy player
            Destroy(player.gameObject);

            // Destroy effect
            Destroy(effect, 1);

            // Level failed
            LevelFailed();
        }
    }

    private void LevelFailed()
    {
        gameManager.EndGame();
    }
}
