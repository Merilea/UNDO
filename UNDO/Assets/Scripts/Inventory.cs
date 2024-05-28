using System;
using System.Collections;
using System.Collections.Generic;
using UNDO;
using UnityEngine;

namespace UNDO
{
    [Serializable]
    public struct Item
    {
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;

        public Item ChangeQuantity(int newQuantity)
        {
            return new Item
            {
                quantity = newQuantity,
                item = this.item
            };
        }
    }

    public class Inventory : MonoBehaviour
    {
        #region Singleton
        public static Inventory instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        [SerializeField] private ItemFactory factory; // Ensure ItemFactory is recognized
        [SerializeField] private GameObject slotsParent;
        [HideInInspector] public int maxSlots = 0;
        public List<Item> items;
        public int spaceUsed = 0;

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallBack;

        private string failMessage = "Cannot add this item to the inventory";

        [SerializeField] private Transform player;

        private ItemSO itemToPlace;
        private GameObject placementIndicator;
        private bool isPlacingItem;

        private void Start()
        {
            maxSlots = slotsParent.transform.childCount;
            items = new List<Item>(maxSlots);
        }

        public bool Add(ItemSO item, int quantity)
        {
            Item newItem = new Item
            {
                quantity = quantity,
                item = item
            };

            if (newItem.item.isStackable)
            {
                int index = IsStored(newItem);
                if (index >= 0 && ((newItem.quantity + items[index].quantity) <= (newItem.item.maxStackAmount)))
                {
                    // Stacking items in the same slot
                    items[index] = newItem.ChangeQuantity((newItem.quantity) + (items[index].quantity));
                    onItemChangedCallBack?.Invoke();
                    return true;
                }
                else if (items.Count + 1 > maxSlots)
                {
                    // Not enough space
                    FailMessageUI();
                    return false;
                }
                else
                {
                    // Add an item to a new slot
                    AddOnANewSlot(newItem);
                    return true;
                }
            }
            else if (items.Count + 1 <= maxSlots)
            {
                // Add an item to a new slot
                AddOnANewSlot(newItem);
                return true;
            }
            else
            {
                // Inventory doesn't have space for an unstackable item
                FailMessageUI();
                return false;
            }
        }

        internal void AddOnANewSlot(Item newItem)
        {
            items.Add(newItem);
            onItemChangedCallBack?.Invoke();
        }

        void FailMessageUI()
        {
            InteractionMessageBehavior.instance.SetText(failMessage, MessageType.Fail);
            StartCoroutine(FailFade());
        }

        IEnumerator FailFade()
        {
            yield return new WaitForSecondsRealtime(2);
            InteractionMessageBehavior.instance.DeactiveText(MessageType.Fail);
        }

        public int IsStored(ItemSO newItem)
        {
            return IsStored(new Item { quantity = 1, item = newItem });
        }

        public int IsStored(Item newItem)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == newItem.item)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemoveItem(ItemSO item)
        {
            items.RemoveAt(IsStored(new Item { quantity = 0, item = item }));
            onItemChangedCallBack?.Invoke();
        }

        public void StartPlacement(ItemSO item)
        {
            itemToPlace = item;
            isPlacingItem = true;
            CreatePlacementIndicator(item);
        }

        private void CreatePlacementIndicator(ItemSO item)
        {
            if (placementIndicator != null)
            {
                Destroy(placementIndicator);
            }

            GameObject itemPrefab = factory.getItem(item.itemType);
            if (itemPrefab != null)
            {
                placementIndicator = Instantiate(itemPrefab);
                DisablePhysics(placementIndicator);
            }
        }

        private void DisablePhysics(GameObject obj)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        private void Update()
        {
            if (isPlacingItem)
            {
                UpdatePlacement();
            }
        }

        private void UpdatePlacement()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                placementIndicator.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    PlaceItem(itemToPlace, hit.point); // Pass itemToPlace and position
                }
            }
        }

        public void PlaceItem(ItemSO item, Vector3 position)
        {
            GameObject itemPrefab = factory.getItem(item.itemType);
            if (itemPrefab != null)
            {
                GameObject placedItem = Instantiate(itemPrefab, position, Quaternion.identity);
                EnablePhysics(placedItem);
                RemoveItem(item);
                isPlacingItem = false;
                Destroy(placementIndicator);
            }
        }

        private void EnablePhysics(GameObject obj)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true;
            }
        }

        public void Consume(ItemSO item, int quantity)
        {
            RemoveQuantity(item, quantity);
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(quantity * 10); // Example: each item restores 10 health
            }
        }

        void RemoveQuantity(ItemSO item, int quantity)
        {
            int index = IsStored(item);
            if (index >= 0)
            {
                int newQuantity = items[index].quantity - quantity;
                items[index] = items[index].ChangeQuantity(newQuantity);
                if (items[index].quantity <= 0)
                {
                    RemoveItem(item);
                }
                onItemChangedCallBack?.Invoke();
            }
        }
    }
}
