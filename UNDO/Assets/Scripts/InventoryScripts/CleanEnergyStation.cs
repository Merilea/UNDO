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
                // Pick up the placed item
                Inventory.instance.Add(item, 1);
                Destroy(gameObject);
            }
            else
            {
                Inventory.instance.Add(item, 1);
                gameObject.SetActive(false);
            }
        }

        public void Place()
        {
            gameObject.SetActive(true);
            isPlaced = true;
        }
    }
}
