using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    // This method will be called when the button is pressed
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
