using UnityEngine;

public class GameOverTriggers : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Called whenever current objects collides with something (Needs a rigid boy + collider)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.EndGame();
        }
    }
}
