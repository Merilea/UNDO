using System;
using System.Collections;
using System.Collections.Generic;
using UNDO;
using UnityEngine;
using TMPro;

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

        [SerializeField] private ItemFactory factory;
        [SerializeField] private GameObject slotsParent;
        [SerializeField] private TextMeshProUGUI placementText;
        [SerializeField] private InventoryUi inventoryUI;
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

            if (placementText != null)
            {
                placementText.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("PlacementText reference is not set in the Inspector.");
            }
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
                    items[index] = newItem.ChangeQuantity((newItem.quantity) + (items[index].quantity));
                    onItemChangedCallBack?.Invoke();
                    return true;
                }
                else if (items.Count + 1 > maxSlots)
                {
                    FailMessageUI();
                    return false;
                }
                else
                {
                    AddOnANewSlot(newItem);
                    return true;
                }
            }
            else if (items.Count + 1 <= maxSlots)
            {
                AddOnANewSlot(newItem);
                return true;
            }
            else
            {
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
            int index = IsStored(item);
            if (index >= 0)
            {
                items.RemoveAt(index);
                onItemChangedCallBack?.Invoke();
            }
        }

        public void StartPlacement(ItemSO item)
        {
            itemToPlace = item;
            Debug.Log("Starting placement for item: " + itemToPlace.name);
            isPlacingItem = true;
            CreatePlacementIndicator(item);
            ShowPlacementText();
            inventoryUI.ToggleInventory();
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
                EnableGameObject(placementIndicator);
                DisablePhysics(placementIndicator);
                Debug.Log("Placement indicator created for item: " + item.name);
            }
            else
            {
                Debug.LogError("Failed to get item prefab for placement indicator.");
            }
        }

        private void EnableGameObject(GameObject obj)
        {
            obj.SetActive(true);
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(true);
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
                if (Input.GetKeyDown(KeyCode.X))
                {
                    CancelPlacement();
                }
            }
        }

        private void UpdatePlacement()
        {
            if (placementIndicator == null)
            {
                Debug.LogError("Placement indicator is null.");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = hit.point;
                newPosition.y = 5.5f; // Ensure consistent height
                placementIndicator.transform.position = newPosition;
                Debug.Log("Placement indicator position updated to: " + newPosition);

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                {
                    PlaceItem(itemToPlace, newPosition);
                }
            }
            else
            {
                Debug.LogError("Raycast did not hit any surface.");
            }
        }

        public void PlaceItem(ItemSO item, Vector3 position)
        {
            GameObject itemPrefab = factory.getItem(item.itemType);
            if (itemPrefab != null)
            {
                Vector3 adjustedPosition = new Vector3(position.x, 5.5f, position.z); // Ensure consistent height
                GameObject placedItem = Instantiate(itemPrefab, adjustedPosition, Quaternion.identity);
                EnableGameObject(placedItem);
                RemoveItem(item);
                isPlacingItem = false;
                Destroy(placementIndicator);
                HidePlacementText();
                inventoryUI.ToggleInventory();
                Debug.Log("Item placed at position: " + adjustedPosition);

                CleanEnergyStation cleanEnergyStation = placedItem.GetComponent<CleanEnergyStation>();
                if (cleanEnergyStation != null)
                {
                    cleanEnergyStation.Place();
                    SolarPanelTask solarPanelTask = FindObjectOfType<SolarPanelTask>();
                    if (solarPanelTask != null)
                    {
                        solarPanelTask.CheckPlacement(adjustedPosition, cleanEnergyStation);
                    }
                    Debug.Log("CleanEnergyStation placed.");
                }
            }
            else
            {
                Debug.LogError("Failed to get item prefab for placing item.");
            }
        }

        private void CancelPlacement()
        {
            isPlacingItem = false;
            if (placementIndicator != null)
            {
                Destroy(placementIndicator);
            }
            HidePlacementText();
            inventoryUI.ToggleInventory();
            Debug.Log("Placement cancelled.");
        }

        private void ShowPlacementText()
        {
            if (placementText != null)
            {
                placementText.gameObject.SetActive(true);
            }
        }

        private void HidePlacementText()
        {
            if (placementText != null)
            {
                placementText.gameObject.SetActive(false);
            }
        }

        public void Consume(ItemSO item, int quantity)
        {
            RemoveQuantity(item, quantity);
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(quantity * 10);
            }
        }

        void RemoveQuantity(ItemSO item, int quantity)
        {
            int index = IsStored(item);
            if (index >= 0)
            {
                int newQuantity = items[index].quantity - quantity;
                if (newQuantity > 0)
                {
                    items[index] = items[index].ChangeQuantity(newQuantity);
                }
                else
                {
                    items.RemoveAt(index);
                }
                onItemChangedCallBack?.Invoke();
            }
        }
    }
}
