using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float health;
    public float stamina;
    public float staminaRegenRate = 5f; // Stamina regeneration rate per second
    public float healthDecayRate = 0.1f; // Health decay rate per second, set to a slower rate

    public Slider healthSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public Image staminaFillImage;
    public Image damageOverlay; // Reference to the red flash overlay
    public float flashDuration = 0.5f; // Duration of the red flash effect
    public float maxFlashAlpha = 0.3f; // Maximum alpha value for the flash effect

    private bool isTakingDamage = false;
    private float damagePerSecond = 0f;
    private Coroutine flashCoroutine;

    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;

        if (staminaFillImage != null)
        {
            staminaFillImage.color = Color.blue;
            RectTransform fillRect = staminaFillImage.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(1, 1);
            fillRect.offsetMin = new Vector2(0, 0);
            fillRect.offsetMax = new Vector2(0, 0);
        }

        if (damageOverlay != null)
        {
            // Initial color: Fully transparent red (#FF000000)
            damageOverlay.color = new Color(1, 0, 0, 0);
            Debug.Log("Damage overlay color set to fully transparent at start."); // Debug log
        }

        UpdateHealthUI();
        UpdateStaminaUI();
    }

    void Update()
    {
        if (stamina < maxStamina && !IsPerformingAction())
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            UpdateStaminaUI();
        }

        if (isTakingDamage)
        {
            health -= damagePerSecond * Time.deltaTime;
            Debug.Log("Health is taking damage. Current health: " + health); // Debug log
            if (health < 0)
            {
                health = 0;
                OnPlayerDeath();
            }
            UpdateHealthUI();
            FlashRed(); // Trigger the red flash effect
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

    bool IsPerformingAction()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Jump");
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player has died.");
        SceneManager.LoadScene("GameOverScene");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("TakeDamage called. Damage: " + damage + ", Current health: " + health); // Debug log
        if (health < 0)
        {
            health = 0;
            OnPlayerDeath();
        }
        UpdateHealthUI();
        FlashRed(); // Trigger the red flash effect
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

    public void StartTakingDamage(float damageRate)
    {
        isTakingDamage = true;
        damagePerSecond = damageRate;
        Debug.Log("Started taking damage: " + damageRate + " per second."); // Debug log
        FlashRed(); // Trigger the red flash effect when starting to take damage
    }

    public void StopTakingDamage()
    {
        isTakingDamage = false;
        damagePerSecond = 0f;
        Debug.Log("Stopped taking damage."); // Debug log
    }

    void FlashRed()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(RedFlashCoroutine());
        Debug.Log("FlashRed called."); // Debug log
    }

    IEnumerator RedFlashCoroutine()
    {
        Debug.Log("RedFlashCoroutine started."); // Debug log
        // Start with semi-transparent red
        damageOverlay.color = new Color(1, 0, 0, maxFlashAlpha);
        Debug.Log("Overlay color set to semi-transparent red at start."); // Debug log

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(maxFlashAlpha, 0f, elapsed / flashDuration);
            damageOverlay.color = new Color(1, 0, 0, alpha);
            Debug.Log("Overlay alpha: " + alpha); // Debug log
            yield return null;
        }
        // Ensure fully transparent at the end
        damageOverlay.color = new Color(1, 0, 0, 0);
        Debug.Log("RedFlashCoroutine ended."); // Debug log
    }
}
