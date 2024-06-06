using UNDO;
using UnityEngine;

namespace UNDO
{
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
            else
            {
                Inventory.instance.Add(item, 1);
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
}
