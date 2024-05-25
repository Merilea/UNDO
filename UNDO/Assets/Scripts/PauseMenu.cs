using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    void Update()
    {
        // Check if the Escape key is pressed and the shop and letter image are not active
        if (Input.GetKeyDown(KeyCode.Escape))// && !LaptopInteraction.isShopMenuActive && !LetterPopUp.isLetterImageActive)
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuCanvas != null)
        {
            bool isPaused = !pauseMenuCanvas.activeSelf;
            pauseMenuCanvas.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f; // Pause or resume the game accordingly

            // Lock or unlock the cursor based on pause state
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused; // Show or hide the cursor based on pause state
        }
    }

    public void ResumeGame()
    {
        // Toggle the visibility of the pause menu canvas
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false); // Hide the pause menu canvas
            Time.timeScale = 1f; // Resume the game by setting time scale to 1

            // Lock the cursor again when the game resumes
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // Hide the cursor again
        }
    }

    public void ReturnToMainMenu()
    {
        // Hide the pause menu if it's active
        if (pauseMenuCanvas != null && pauseMenuCanvas.activeSelf)
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f; // Ensure the game resumes if the pause menu was active
        }

        // Return to the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowHelp()
    {
        // You can display help instructions here or navigate to another scene with detailed instructions
        Debug.Log("Help: Use WASD to walk."); // Placeholder for displaying help
    }
}