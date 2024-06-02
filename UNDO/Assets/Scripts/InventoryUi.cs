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
        private PlayerMove playerMove;

        void Start()
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallBack += UpdateUI;
            playerMove = FindObjectOfType<PlayerMove>();

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

            // Set cursor lock state and visibility based on inventory state
            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = true; // Ensure the cursor is always visible

            // Update the PlayerMove script with the current inventory state
            if (playerMove != null)
            {
                playerMove.isInventoryOpen = !isActive;
            }
        }

        public bool IsInventoryActive()
        {
            return inventoryCanvas.activeSelf;
        }
    }
}
