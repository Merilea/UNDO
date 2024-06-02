using UnityEngine;

public class InspectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 2f; // Speed at which the camera zooms in
    public float zoomInFOV = 30f; // Target FOV for zoom-in (narrower view)
    public float zoomOutFOV = 60f; // Original FOV
    public float zoomInDistance = 0.5f; // Distance to move the camera forward for inspection

    private bool isInspecting;
    private Transform target;
    private Vector3 originalLocalPosition;
    private Vector3 inspectionPosition;

    void Start()
    {
        // Store the original local position of the camera
        originalLocalPosition = mainCamera.transform.localPosition;
    }

    void Update()
    {
        if (isInspecting)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomInFOV, zoomSpeed * Time.deltaTime);

            // Move the camera slightly forward for a better inspection view
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, inspectionPosition, zoomSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopInspection();
            }
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomOutFOV, zoomSpeed * Time.deltaTime);
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, originalLocalPosition, zoomSpeed * Time.deltaTime);
        }

        if (isInspecting)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void StartInspection(Transform inspectionTarget)
    {
        isInspecting = true;
        target = inspectionTarget;

        // Calculate the inspection position based on the target's position
        Vector3 directionToTarget = (target.position - mainCamera.transform.position).normalized;
        inspectionPosition = originalLocalPosition + directionToTarget * zoomInDistance;

        Time.timeScale = 0; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopInspection()
    {
        isInspecting = false;
        target = null;
        Time.timeScale = 1; // Resume the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public bool IsInspecting()
    {
        return isInspecting;
    }
}
