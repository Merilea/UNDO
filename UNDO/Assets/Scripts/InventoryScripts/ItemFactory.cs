using UnityEngine;

namespace UNDO
{
    public class ItemFactory : MonoBehaviour
    {
        public GameObject getItem(ItemType itemType)
        {
            Debug.Log("Searching for item type: " + itemType); // Debug log to check itemType

            foreach (var itemSO in Resources.LoadAll<ItemSO>(""))
            {
                Debug.Log("Checking item: " + itemSO.name); // Debug log to check each itemSO

                if (itemSO.itemType == itemType)
                {
                    if (itemSO.prefab != null)
                    {
                        Debug.Log("Found prefab for item: " + itemSO.name); // Debug log to confirm finding prefab
                        return itemSO.prefab;
                    }
                    else
                    {
                        Debug.LogError("Prefab is null for item: " + itemSO.name); // Debug log to catch null prefab
                    }
                }
            }
            Debug.LogError("Item type not found: " + itemType); // Debug log if itemType not found
            return null;
        }
    }
}
