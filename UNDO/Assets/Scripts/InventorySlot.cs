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
        [SerializeField] private GameObject dropButton;
        private int amount = 0;

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

            dropButton.SetActive(true);
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

            dropButton.SetActive(false);
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
            if (_item.item.itemType == ItemType.Consumable)
            {
                Inventory.instance.Consume(_item.item, _item.quantity);
                ClearSlot();
            }
        }

        public void PlaceItem()
        {
            if (_item.item.itemType == ItemType.CleanEnergyStation)
            {
                _item.item.StartPlacement();
                ClearSlot();
            }
        }
    }
}
