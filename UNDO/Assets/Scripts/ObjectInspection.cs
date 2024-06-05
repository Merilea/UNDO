using UnityEngine;
using TMPro;

public class ObjectInspection : MonoBehaviour
{
    public float zoomFOV = 20f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;
    public float maxInspectionDistance = 3f;
    public Texture2D hoverCursor;
    public Texture2D defaultCursor;
    public TextMeshProUGUI inspectionText; // Reference to the UI Text element

    private Camera playerCamera;
    private bool isZoomed = false;
    private GameObject currentObject;

    private float debounceTime = 0.2f; // Cooldown period
    private float lastZoomToggleTime = 0f;

    void Start()
    {
        playerCamera = Camera.main;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        if (inspectionText != null)
        {
            inspectionText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("InspectionText reference is not set in the Inspector.");
        }
    }

    void Update()
    {
        HandleRaycast();
        HandleZoom();
    }

    void HandleRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Inspectable"))
            {
                float distanceToObject = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToObject <= maxInspectionDistance)
                {
                    Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);

                    if (!isZoomed) // Show text only if not already zoomed
                    {
                        ShowInspectionText(hit);
                    }

                    if (Input.GetKeyDown(KeyCode.E) && Time.time > lastZoomToggleTime + debounceTime)
                    {
                        isZoomed = !isZoomed; // Toggle zoom state
                        currentObject = hit.collider.gameObject;
                        lastZoomToggleTime = Time.time; // Update the last toggle time
                        HideInspectionText(); // Hide the inspection text when starting the inspection
                        if (!isZoomed)
                        {
                            StopInspection();
                        }
                        Debug.Log("Zoom toggled: " + isZoomed);
                    }
                }
                else
                {
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                    HideInspectionText();
                }
            }
            else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                HideInspectionText();
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            HideInspectionText();
        }
    }

    void HandleZoom()
    {
        if (isZoomed)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Escape) || (currentObject != null && Input.GetKeyDown(KeyCode.E) && Time.time > lastZoomToggleTime + debounceTime))
            {
                isZoomed = false;
                lastZoomToggleTime = Time.time; // Update the last toggle time
                StopInspection(); // Immediately reset FOV to normal when inspection stops
                Debug.Log("Stopped inspecting");
            }
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
        }
    }

    void ShowInspectionText(RaycastHit hit)
    {
        if (inspectionText != null && !inspectionText.gameObject.activeSelf)
        {
            inspectionText.gameObject.SetActive(true);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);
            inspectionText.transform.position = screenPosition;
            Debug.Log("Showing inspection text at: " + screenPosition);
        }
    }

    void HideInspectionText()
    {
        if (inspectionText != null && inspectionText.gameObject.activeSelf)
        {
            inspectionText.gameObject.SetActive(false);
            Debug.Log("Hiding inspection text");
        }
    }

    public bool IsInspecting()
    {
        return isZoomed;
    }

    public void StopInspection()
    {
        isZoomed = false;
        ResetFOV();
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        HideInspectionText(); // Ensure the text is hidden when stopping inspection
        Debug.Log("Inspection stopped and text hidden");
    }

    private void ResetFOV()
    {
        playerCamera.fieldOfView = normalFOV;
        Debug.Log("FOV reset to normal");
    }
}
