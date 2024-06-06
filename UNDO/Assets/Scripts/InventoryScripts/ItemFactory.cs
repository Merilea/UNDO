using UnityEngine;

namespace UNDO
{
    public class ItemFactory : MonoBehaviour
    {
        public GameObject getItem(ItemType item)
        {
            Debug.Log("Searching for item type: " + item);
            foreach (var itemSO in Resources.LoadAll<ItemSO>(""))
            {
                Debug.Log("Checking item: " + itemSO.name);
                if (itemSO.itemType == item)
                {
                    Debug.Log("Found prefab for item: " + itemSO.name);
                    return itemSO.prefab;
                }
            }
            Debug.LogError("Item type not found: " + item);
            return null;
        }
    }
}
