using UNDO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Static variable to track pause state
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

        // Manage cursor state based on the current UI state
        if (pauseMenuCanvas.activeSelf || inventoryUi.IsInventoryActive())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true; // Ensure the cursor is always visible
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuCanvas != null)
        {
            GameIsPaused = !pauseMenuCanvas.activeSelf;
            pauseMenuCanvas.SetActive(GameIsPaused);
            Time.timeScale = GameIsPaused ? 0f : 1f; // Pause or resume the game accordingly

            // Ensure the cursor is visible and manage its lock state
            Cursor.lockState = GameIsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuCanvas != null)
        {
            GameIsPaused = false;
            pauseMenuCanvas.SetActive(false); // Hide the pause menu canvas
            Time.timeScale = 1f; // Resume the game by setting time scale to 1

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true; // Ensure the cursor is always visible
        }
    }

    public void ReturnToMainMenu()
    {
        if (pauseMenuCanvas != null && pauseMenuCanvas.activeSelf)
        {
            GameIsPaused = false;
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

