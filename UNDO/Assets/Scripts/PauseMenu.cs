using UnityEngine;
using UnityEngine.SceneManagement;
using UNDO;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public InventoryUi inventoryUi;

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryUi.IsInventoryActive())
            {
                inventoryUi.ToggleInventory();
            }
            else
            {
                TogglePauseMenu();
            }
        }

        // Check if the 'I' or 'Tab' key is pressed to toggle the inventory
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (!pauseMenuCanvas.activeSelf)
            {
                inventoryUi.ToggleInventory();
            }
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
            Cursor.visible = isPaused;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false); // Hide the pause menu canvas
            Time.timeScale = 1f; // Resume the game by setting time scale to 1

            // Lock the cursor again when the game resumes
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // Hide the cursor
        }
    }

    public void ReturnToMainMenu()
    {
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
