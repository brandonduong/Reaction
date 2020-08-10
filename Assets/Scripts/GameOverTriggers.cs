using UnityEngine;

public class GameOverTriggers : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // If player falls below camera sight (down a hole), game over
        if (gameObject.GetComponent<Rigidbody2D>().position.y < -8.55f)
        {
            gameManager.EndGame();
        }
    }

    // Called whenever current objects collides with something (Needs a rigid boy + collider)
    void OnCollisionEnter(Collision collision)
    {

    }
}
