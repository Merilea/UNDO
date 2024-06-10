using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public Animator creditsAnimator;

    void Start()
    {
        creditsAnimator.Play("CreditsScroll");
        Invoke("LoadMainMenu", 20f); // Load MainMenu after 20 seconds
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
