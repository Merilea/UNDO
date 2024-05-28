using UNDO;
using UnityEngine;

namespace UNDO
{
    public class PickUpBehaviour : MonoBehaviour
    {
        public ItemSO item;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Inventory.instance.Add(item, 1);
                Destroy(gameObject);
            }
        }
    }
}
