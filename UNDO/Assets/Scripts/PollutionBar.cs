using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PollutionLevel : MonoBehaviour
{
    public static PollutionLevel Instance; // Singleton instance

    public Slider pollutionSlider;
    public TextMeshProUGUI pollutionText;
    public Image pollutionDecreasedOverlay; // Reference to the overlay image
    private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void DecreasePollution(float amount)
    {
        gameManager.ReducePollution(amount);
        UpdatePollutionUI();
        StartCoroutine(ShowPollutionDecreasedOverlay());
    }

    private IEnumerator ShowPollutionDecreasedOverlay()
    {
        Color originalColor = pollutionDecreasedOverlay.color;
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        Color semiTransparentColor = originalColor;
        semiTransparentColor.a = 0.5f;

        float fadeInDuration = 0.2f; // Duration for fading in
        float holdDuration = 0.5f;   // Duration for holding the semi-transparent state
        float fadeOutDuration = 0.2f;// Duration for fading out

        // Fade in to semi-transparent green
        for (float t = 0; t < 1; t += Time.deltaTime / fadeInDuration)
        {
            pollutionDecreasedOverlay.color = Color.Lerp(transparentColor, semiTransparentColor, t);
            yield return null;
        }

        // Hold for a short duration
        yield return new WaitForSeconds(holdDuration);

        // Fade out to fully transparent
        for (float t = 0; t < 1; t += Time.deltaTime / fadeOutDuration)
        {
            pollutionDecreasedOverlay.color = Color.Lerp(semiTransparentColor, transparentColor, t);
            yield return null;
        }

        pollutionDecreasedOverlay.color = transparentColor; // Ensure it's fully transparent at the end
    }
}

