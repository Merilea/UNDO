using UNDO;
using UnityEngine;

public class CleanEnergyStation : MonoBehaviour
{
    public ItemSO item;
    private bool isPlaced = false;

    public void Interact()
    {
        if (!isPlaced)
        {
            if (Inventory.instance.Add(item, 1))
            {
                Destroy(gameObject); // Destroy the game object after adding to inventory
            }
            else
            {
                Debug.Log("Failed to add item to inventory");
            }
        }
    }

    public void Place()
    {
        gameObject.SetActive(true);
        isPlaced = true;
        // Notify task manager
        SolarPanelTask solarPanelTask = FindObjectOfType<SolarPanelTask>();
        if (solarPanelTask != null)
        {
            solarPanelTask.CheckPlacement(transform.position, this);
        }
    }

    public void SetUninteractable()
    {
        isPlaced = true;
        // Additional logic to make it uninteractable
        GetComponent<Collider>().enabled = false;
    }
}
