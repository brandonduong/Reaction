using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public FadeInAndOut fadeManager;
    // Start is called before the first frame update
    void Start()
    {
        // AudioManager.instance.PlaySound("menuSong");
    }

    public void PlayGame()
    {
        // Load next scene in queue
        fadeManager.FadeToScene("LevelSelect");
    }

    public void QuitGame()
    {
        fadeManager.FadeToScene("");
        Debug.Log("Quit Game.");
        Application.Quit();
    }
}
