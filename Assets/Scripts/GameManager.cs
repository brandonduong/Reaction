﻿using System.Collections;
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

            // If game over, restart level
            Restart();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
