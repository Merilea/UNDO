using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Canvas inventoryCanvas;
    public Transform inventoryContent;
    public Button inventoryButtonPrefab;
    public PlayerMove playerMove;

    private List<GameObject> items = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (inventoryCanvas == null)
        {
            Debug.LogError("Inventory Canvas is not assigned in the Inspector");
        }

        if (inventoryButtonPrefab == null)
        {
            Debug.LogError("Inventory Button Prefab is not assigned in the Inspector");
        }

        if (inventoryContent == null)
        {
            Debug.LogError("Inventory Content is not assigned in the Inspector");
        }

        inventoryCanvas.gameObject.SetActive(false); // Start with inventory hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inventoryCanvas.gameObject.activeSelf)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        Debug.Log("ToggleInventory called");
        bool isActive = !inventoryCanvas.gameObject.activeSelf;
        inventoryCanvas.gameObject.SetActive(isActive);
        playerMove.isInventoryOpen = isActive;

        if (isActive)
        {
            // Unlock and show cursor when inventory is open
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Lock and hide cursor when inventory is closed
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool IsInventoryActive()
    {
        return inventoryCanvas.gameObject.activeSelf;
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        Debug.Log("Item added: " + item.name);
        UpdateInventoryUI();
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Debug.Log("Item removed: " + item.name);
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        // Ensure the inventoryContent is assigned
        if (inventoryContent == null)
        {
            Debug.LogError("Inventory Content is not assigned in the Inspector");
            return;
        }

        if (inventoryButtonPrefab == null)
        {
            Debug.LogError("Inventory Button Prefab is not assigned in the Inspector");
            return;
        }

        // Destroy all existing buttons in the inventory UI
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Create a button for each item in the inventory
        foreach (GameObject item in items)
        {
            if (item == null)
            {
                Debug.LogError("Item is null");
                continue;
            }

            Button button = Instantiate(inventoryButtonPrefab, inventoryContent);
            if (button == null)
            {
                Debug.LogError("Failed to instantiate button");
                continue;
            }

            // Ensure the button has a child TMP_Text component
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText == null)
            {
                Debug.LogError("Button TMP_Text component is missing");
                continue;
            }

            buttonText.text = item.name;
            button.onClick.AddListener(() => OnInventoryButtonClicked(item));
        }
    }

    void OnInventoryButtonClicked(GameObject item)
    {
        Debug.Log("Inventory button clicked for: " + item.name);
        ToggleInventory(); // Hide inventory
        PlaceableItem placeableItem = item.GetComponent<PlaceableItem>();
        if (placeableItem != null)
        {
            Debug.Log("Starting to place item: " + item.name);
            placeableItem.StartPlacing(() => RemoveItem(item));
        }
    }
}
