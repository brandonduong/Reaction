using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameWin = true;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EndGame()
    {
        if (!gameOver)
        {
            gameOver = true;

            // If game over, restart level
            Restart();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
