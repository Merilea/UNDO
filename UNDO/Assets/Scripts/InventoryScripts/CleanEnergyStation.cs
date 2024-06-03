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
                Inventory.instance.Add(item, 1);
                gameObject.SetActive(false);
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
