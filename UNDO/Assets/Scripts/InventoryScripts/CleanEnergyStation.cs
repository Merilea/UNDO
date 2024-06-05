using UnityEngine;

namespace UNDO
{
    public class CleanEnergyStation : Interactable
    {
        public ItemSO item;
        private bool isPlaced = false;

        public override void Interact()
        {
            if (isPlaced)
            {
                Debug.Log("Picking up placed CleanEnergyStation.");
                Inventory.instance.Add(item, 1);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Adding CleanEnergyStation to inventory.");
                Inventory.instance.Add(item, 1);
                gameObject.SetActive(false);
            }
        }

        public void Place()
        {
            gameObject.SetActive(true);
            isPlaced = true;
            Debug.Log("CleanEnergyStation placed.");
        }
    }
}
