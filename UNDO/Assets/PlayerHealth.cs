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
    public float staminaRegenRate = 5f; // Stamina regeneration rate per second
    public float healthDecayRate = 0.1f; // Health decay rate per second, set to a slower rate

    public Slider healthSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public Image staminaFillImage;

    private bool isTakingDamage = false;
    private float damagePerSecond = 0f;

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
            if (health < 0)
            {
                health = 0;
                OnPlayerDeath();
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

    public void StartTakingDamage(float damageRate)
    {
        isTakingDamage = true;
        damagePerSecond = damageRate;
    }

    public void StopTakingDamage()
    {
        isTakingDamage = false;
        damagePerSecond = 0f;
    }
}
