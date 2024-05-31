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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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
        // Destroy the GameManager if the current scene is GameOverScene or WinScene
        if (scene.name == "GameOverScene" || scene.name == "WinScene")
        {
            Destroy(gameObject);
        }
    }

    public void ReducePollution(float amount)
    {
        pollutionLevel -= amount;
        if (pollutionLevel < 0)
        {
            pollutionLevel = 0;
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
}
