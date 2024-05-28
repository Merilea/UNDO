using UNDO;
using UnityEngine;

namespace UNDO
{
    public class ItemFactory : MonoBehaviour
    {
        [SerializeField] GameObject consumable;
        [SerializeField] GameObject cleanEnergy;

        public GameObject getItem(ItemType item)
        {
            switch (item)
            {
                case ItemType.Consumable:
                    return consumable;
                case ItemType.CleanEnergyStation:
                    return cleanEnergy;
                default:
                    return null;
            }
        }
    }
}
