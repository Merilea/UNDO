using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float health;
    public float stamina;
    public float pollutionImpact = 1f; // Impact of pollution on health
    public float staminaRegenRate = 5f; // Stamina regeneration rate per second
    public float healthDecayRate = 0.1f; // Health decay rate per second, set to a slower rate

    public Slider healthSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI healthText; // Change to TextMeshProUGUI
    public TextMeshProUGUI staminaText; // Change to TextMeshProUGUI
    public Image staminaFillImage; // Reference to the Image component of the fill area

    void Start()
    {
        // Initialize health and stamina
        health = maxHealth;
        stamina = maxStamina;

        // Set the stamina slider fill color to blue
        if (staminaFillImage != null)
        {
            staminaFillImage.color = Color.blue;
            // Adjust the RectTransform of the Fill Area
            RectTransform fillRect = staminaFillImage.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(1, 1);
            fillRect.offsetMin = new Vector2(0, 0);
            fillRect.offsetMax = new Vector2(0, 0);
        }

        // Initialize UI elements
        UpdateHealthUI();
        UpdateStaminaUI();
    }

    void Update()
    {
        // Handle stamina regeneration
        if (stamina < maxStamina && !IsPerformingAction())
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            UpdateStaminaUI();
        }

        // Continuously decrease health over time
        health -= healthDecayRate * Time.deltaTime;
        if (health < 0)
        {
            health = 0;
            OnPlayerDeath();
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }
        if (healthText != null)
        {
            healthText.text = "Health: " + Mathf.CeilToInt(health);
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina / maxStamina;
        }
        if (staminaText != null)
        {
            staminaText.text = "Stamina: " + Mathf.CeilToInt(stamina);
        }
    }

    bool IsPerformingAction()
    {
        // Add logic to determine if the player is performing an action that should prevent stamina regeneration
        // For instance, running or jumping
        return Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Jump");
    }

    void OnPlayerDeath()
    {
        // Implement logic for player death, such as respawn or game over
        Debug.Log("Player has died.");
        SceneManager.LoadScene("GameOverScene"); // Load the GameOverScene
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            OnPlayerDeath();
        }
        UpdateHealthUI();
    }

    public void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0)
        {
            stamina = 0;
        }
        UpdateStaminaUI();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthUI();
    }

    public void RestoreStamina(float amount)
    {
        stamina += amount;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        UpdateStaminaUI();
    }
}
