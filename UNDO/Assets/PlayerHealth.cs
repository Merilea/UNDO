using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float health;
    public float stamina;
    public float pollutionImpact = 1f; // Impact of pollution on health
    public float staminaRegenRate = 5f; // Stamina regeneration rate per second
    public float healthRegenRate = 1f; // Health regeneration rate per second

    public Slider healthSlider;
    public Slider staminaSlider;
    public Text healthText;
    public Text staminaText;

    void Start()
    {
        // Initialize health and stamina
        health = maxHealth;
        stamina = maxStamina;

        // Initialize UI elements
        UpdateHealthUI();
        UpdateStaminaUI();
    }

    void Update()
    {
        // Handle stamina regeneration
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            UpdateStaminaUI();
        }

        // Handle pollution impact on health
        if (IsInPollutedArea())
        {
            health -= pollutionImpact * Time.deltaTime;
            if (health < 0)
            {
                health = 0;
                // Handle player death (optional)
                OnPlayerDeath();
            }
            UpdateHealthUI();
        }

        // Regenerate health if not in a polluted area
        if (health < maxHealth && !IsInPollutedArea())
        {
            health += healthRegenRate * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UpdateHealthUI();
        }
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

    bool IsInPollutedArea()
    {
        // Implement logic to check if the player is in a polluted area
        // This could be based on triggers, colliders, or pollution levels in the environment
        return false; // Placeholder
    }

    void OnPlayerDeath()
    {
        // Implement logic for player death, such as respawn or game over
        Debug.Log("Player has died.");
        SceneManager.LoadScene("GameOverScene");
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
