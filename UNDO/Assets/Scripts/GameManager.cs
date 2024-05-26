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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check for win condition
        if (pollutionLevel <= pollutionReductionTarget && cleanEnergyProjects >= requiredCleanEnergyProjects)
        {
            WinGame();
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
        SceneManager.LoadScene("WinScene");
    }

    public void GameOver()
    {
        // Show game over screen
        SceneManager.LoadScene("GameOverScene");
    }
}
