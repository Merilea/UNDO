using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float pollutionLevel = 80f; // Start with high pollution
    public float maxPollutionLevel = 100f;
    public float pollutionReductionTarget = 20f;
    public int cleanEnergyProjects = 0;
    public int requiredCleanEnergyProjects = 5;

    private float initialPollutionLevel;
    private int initialCleanEnergyProjects;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Save initial values
            initialPollutionLevel = pollutionLevel;
            initialCleanEnergyProjects = cleanEnergyProjects;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unlock and make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reset game state when SampleScene is loaded
        if (scene.name == "SampleScene")
        {
            ResetGameState();
        }
    }

    public void ReducePollution(float amount)
    {
        pollutionLevel -= amount;
        if (pollutionLevel <= 0)
        {
            pollutionLevel = 0;
        }

        // Check if pollution level reaches the target for winning the game
        if (pollutionLevel <= 50)
        {
            WinGame();
        }
    }

    public void AddCleanEnergyProject()
    {
        cleanEnergyProjects++;
    }

    void WinGame()
    {
        // Show win screen
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        // Show game over screen
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        // Reload the game scene without resetting the game state
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void ResetGameState()
    {
        pollutionLevel = initialPollutionLevel;
        cleanEnergyProjects = initialCleanEnergyProjects;
    }
}
