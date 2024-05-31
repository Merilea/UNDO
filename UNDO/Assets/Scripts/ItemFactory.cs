using UnityEngine;

namespace UNDO
{
    public class ItemFactory : MonoBehaviour
    {
        public GameObject getItem(ItemType item)
        {
            foreach (var itemSO in Resources.LoadAll<ItemSO>(""))
            {
                if (itemSO.itemType == item)
                {
                    return itemSO.prefab;
                }
            }
            return null;
        }
    }
}
