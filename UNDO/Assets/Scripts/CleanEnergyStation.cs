using UnityEngine;

public class CleanEnergyStation : Interactable
{
    public float pollutionReductionAmount = 10f;
    private bool isPlaced = false;

    public override void Interact()
    {
        if (!isPlaced)
        {
            base.Interact();
            InventoryManager.Instance.AddItem(this.gameObject);
            gameObject.SetActive(false); // Deactivate instead of destroying
        }
        else
        {
            // Logic for picking up the item again, if already placed
            InventoryManager.Instance.AddItem(this.gameObject);
            gameObject.SetActive(false);
            isPlaced = false;
        }
    }

    public void Place()
    {
        gameObject.SetActive(true);
        isPlaced = true;
    }
}
