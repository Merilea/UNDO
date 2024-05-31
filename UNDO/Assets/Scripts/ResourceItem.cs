using UnityEngine;

namespace UNDO
{
    public class ResourceItem : Interactable
    {
        public ItemSO item;
        public int quantity = 1;

        public override void Interact()
        {
            base.Interact();
            AddToInventory();
        }

        private void AddToInventory()
        {
            Inventory inventory = Inventory.instance;
            if (inventory != null)
            {
                if (inventory.Add(item, quantity))
                {
                    Debug.Log("Item added to inventory: " + item.name);
                    Destroy(gameObject); // Remove the item from the world after adding to inventory
                }
            }
        }
    }
}
