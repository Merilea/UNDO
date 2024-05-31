using UnityEngine;

namespace UNDO
{

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        public ItemType itemType;
        new public string name = "New item";
        public GameObject prefab; // Reference to the 3D GameObject
        public Sprite icon; // Reference to the inventory icon
        public int size = 1;
        public string interactText = "Press E to pickup ";
        public string description = "";
        public bool isStackable = false;
        public int maxStackAmount = 3;

        public virtual void Use()
        {
            Debug.Log("Item " + name + " is being used");
        }

        public void StartPlacement()
        {
            Inventory.instance.StartPlacement(this);
        }
    }
}
