using UnityEngine;
using System;

public class PlaceableItem : MonoBehaviour
{
    private bool isPlacing = false;
    private GameObject placeholder;
    private Action onPlacedCallback;

    void Update()
    {
        if (isPlacing)
        {
            PlaceObject();
        }
    }

    public void StartPlacing(Action onPlaced)
    {
        Debug.Log("StartPlacing called for: " + gameObject.name);
        isPlacing = true;
        onPlacedCallback = onPlaced;

        // Create a placeholder object to visualize placement
        placeholder = Instantiate(gameObject);
        placeholder.GetComponent<Collider>().enabled = false; // Disable collision on placeholder
        Renderer[] renderers = placeholder.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                Color originalColor = renderer.material.color;
                renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); // Make placeholder semi-transparent
            }
            else
            {
                Debug.LogError("Renderer is missing on placeholder object.");
            }
        }
        Debug.Log("Placeholder created for: " + gameObject.name);
    }

    void PlaceObject()
    {
        // Update placeholder position to follow the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Raycast from camera position: " + ray.origin + " in direction: " + ray.direction);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            placeholder.transform.position = hit.point;
            Debug.Log("Placeholder position updated to: " + hit.point);
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }

        // Confirm placement with left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(gameObject, placeholder.transform.position, Quaternion.identity);
            Debug.Log("Object instantiated at: " + placeholder.transform.position);
            Destroy(placeholder);
            isPlacing = false;
            onPlacedCallback?.Invoke();
        }

        // Cancel placement with right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(placeholder);
            isPlacing = false;
            Debug.Log("Placement cancelled");
        }
    }
}

