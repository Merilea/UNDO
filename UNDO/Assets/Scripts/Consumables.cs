using UnityEngine;
using UNDO;

namespace UNDO
{
    public class Consumable : MonoBehaviour
    {
        public ItemSO item;

        public void Use()
        {
            Debug.Log("Consuming " + item.name);
        }
    }
}
