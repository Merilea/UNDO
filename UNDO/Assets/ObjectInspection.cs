using UnityEngine;

public class ObjectInspection : MonoBehaviour
{
    public float zoomFOV = 20f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;
    public float maxInspectionDistance = 3f;
    public Texture2D hoverCursor;
    public Texture2D defaultCursor;
    private Camera playerCamera;
    private bool isZoomed = false;
    private GameObject currentObject;

    private float debounceTime = 0.2f; // Cooldown period
    private float lastZoomToggleTime = 0f;

    void Start()
    {
        playerCamera = Camera.main;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
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

                    if (Input.GetKeyDown(KeyCode.E) && Time.time > lastZoomToggleTime + debounceTime)
                    {
                        isZoomed = !isZoomed; // Toggle zoom state
                        currentObject = hit.collider.gameObject;
                        lastZoomToggleTime = Time.time; // Update the last toggle time
                        if (!isZoomed)
                        {
                            ResetFOV(); // Immediately reset FOV to normal when inspection stops
                        }
                        Debug.Log("Zoom toggled: " + isZoomed);
                    }
                }
                else
                {
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
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
                ResetFOV(); // Immediately reset FOV to normal when inspection stops
                Debug.Log("Stopped inspecting");
            }
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
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
    }

    private void ResetFOV()
    {
        playerCamera.fieldOfView = normalFOV;
        Debug.Log("FOV reset to normal");
    }
}
