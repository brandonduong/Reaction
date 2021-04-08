using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    public static GameObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = Instantiate(pauseMenu);
        }
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        instance.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        instance.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        instance.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        instance.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        FindObjectOfType<FadeInAndOut>().FadeToScene("");
        Debug.Log("Quit Game.");
        Application.Quit();
    }
}
