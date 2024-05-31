using UNDO;
using UnityEngine;

namespace UNDO
{
    public class InventoryUi : MonoBehaviour
    {
        Inventory inventory;
        [SerializeField] private GameObject slotsParent;
        [SerializeField] private GameObject inventoryCanvas;
        private InventorySlot[] slots;

        void Start()
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallBack += UpdateUI;

            slots = slotsParent.GetComponentsInChildren<InventorySlot>();
            ClearSlots();
            inventoryCanvas.SetActive(false); // Make sure inventory starts off
        }

        void UpdateUI()
        {
            ClearSlots();
            for (int i = 0; i < inventory.items.Count; i++)
            {
                slots[i].AddItem(inventory.items[i]);
            }
        }

        void ClearSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].ClearSlot();
            }
        }

        public void ToggleInventory()
        {
            bool isActive = inventoryCanvas.activeSelf;
            inventoryCanvas.SetActive(!isActive);
            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isActive;
        }

        public bool IsInventoryActive()
        {
            return inventoryCanvas.activeSelf;
        }
    }
}

