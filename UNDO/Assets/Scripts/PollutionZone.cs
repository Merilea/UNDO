using UnityEngine;

public class PollutionZone : MonoBehaviour
{
    public float damagePerSecond = 10f; // Damage per second when inside the pollution zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.StartTakingDamage(damagePerSecond);
                Debug.Log("Player entered pollution zone."); // Debug log
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.StopTakingDamage();
                Debug.Log("Player exited pollution zone."); // Debug log
            }
        }
    }
}
