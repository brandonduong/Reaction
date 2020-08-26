using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInAndOut : MonoBehaviour
{
    private Animator animator;

    private string sceneToLoad;
    private int sceneToLoadIndex;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeToScene(string sceneName)
    {
        sceneToLoad = sceneName;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToScene(int sceneIndex)
    {
        sceneToLoadIndex = sceneIndex;
        sceneToLoad = "";
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        if (sceneToLoad.Length > 0)
        {
            Debug.Log($"Fading to {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoadIndex);
        }

        sceneToLoad = "";
    }
}
