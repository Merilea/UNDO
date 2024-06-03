using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PollutionLevel : MonoBehaviour
{
    public Slider pollutionSlider;
    public TextMeshProUGUI pollutionText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (pollutionSlider == null)
        {
            pollutionSlider = GetComponent<Slider>();
        }

        pollutionSlider.maxValue = gameManager.maxPollutionLevel;
        UpdatePollutionUI();
    }

    void Update()
    {
        UpdatePollutionUI();
    }

    void UpdatePollutionUI()
    {
        if (pollutionSlider != null)
        {
            pollutionSlider.value = gameManager.pollutionLevel;
        }
        if (pollutionText != null)
        {
            pollutionText.text = "Pollution: " + Mathf.CeilToInt(gameManager.pollutionLevel);
        }
    }
}
