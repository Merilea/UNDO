using UNDO;
using UnityEngine;
using UnityEngine.UI;

namespace UNDO
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] public Item _item;

        [SerializeField] private Image icon;
        [SerializeField] private Text itemName;
        [SerializeField] private Text amountDisplay;
        [SerializeField] private GameObject useButton; // Change to use button

        private int amount = 0;

        private void Awake()
        {
            if (useButton == null)
            {
                Debug.LogError("Use Button is not assigned in the Inspector.");
            }
        }

        public void AddItem(Item newItem)
        {
            if (_item.item == null)
            {
                AddNewItem(newItem);
            }
            else if (_item.item == newItem.item)
            {
                StackItem(newItem);
            }
        }

        public void AddNewItem(Item newItem)
        {
            SetItem(newItem);
            SetAmount(_item.quantity);

            icon.sprite = _item.item.icon;
            icon.enabled = true;

            itemName.text = _item.item.name;
            itemName.enabled = true;

            if (useButton != null)
            {
                useButton.SetActive(true); // Enable use button
            }
        }

        public void StackItem(Item newItem)
        {
            SetItem(newItem);
            SetAmount(_item.quantity);
        }

        public void ClearSlot()
        {
            _item.item = null;

            icon.sprite = null;
            icon.enabled = false;

            itemName.text = "";
            itemName.enabled = false;

            if (useButton != null)
            {
                useButton.SetActive(false); // Disable use button
            }
            SetAmount(0);
        }

        public void SetItem(Item newItem)
        {
            _item.item = newItem.item;
            _item = _item.ChangeQuantity(newItem.quantity);
        }

        public void SetAmount(int quantity)
        {
            amount = quantity;
            if (quantity > 0)
                amountDisplay.text = "" + amount;
            else
                amountDisplay.text = "";
        }

        public void UseItem()
        {
            if (_item.item != null)
            {
                if (_item.item.itemType == ItemType.CleanEnergyStation)
                {
                    Inventory.instance.StartPlacement(_item.item); // Start placement process
                }
                else
                {
                    Inventory.instance.Consume(_item.item, 1); // Consume one item at a time
                    if (_item.quantity <= 1)
                    {
                        ClearSlot();
                    }
                    else
                    {
                        SetAmount(_item.quantity - 1);
                        _item = _item.ChangeQuantity(_item.quantity - 1);
                    }
                }
                Inventory.instance.onItemChangedCallBack?.Invoke(); // Ensure UI is updated
            }
        }
    }
}
