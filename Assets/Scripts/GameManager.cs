using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameWin = true;
    private bool gameOver = false;

    public void NextLevel()
    {
        Debug.Log("Level won");
        gameWin = true;

        // Pops up the level complete screen
        // levelCompleteUI.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
        {
            Restart();
            return;
        }

        // Load next scene in queue (Presumably the next level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        if (!gameOver)
        {
            gameOver = true;

            // Destroy player
            Destroy(FindObjectOfType<PlayerController2D>().gameObject);

            // Audio
            AudioManager.instance.PlaySound("PlayerDeath");

            // If game over, restart level
            Invoke(nameof(Restart), 1f);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
