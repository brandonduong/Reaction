using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // AudioManager.instance.PlaySound("menuSong");
    }

    public void PlayGame()
    {
        // Load next scene in queue
        FindObjectOfType<FadeInAndOut>().FadeToScene("LevelSelect");
    }

    public void QuitGame()
    {
        FindObjectOfType<FadeInAndOut>().FadeToScene("");
        Debug.Log("Quit Game.");
        Application.Quit();
    }
}
