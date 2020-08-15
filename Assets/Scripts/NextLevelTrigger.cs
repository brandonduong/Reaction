using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.NextLevel();
        }
    }
}
