using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    private CursorManager cursorManager;
    private InspectionManager inspectionManager;
    private bool isHovered = false;

    void Start()
    {
        cursorManager = FindObjectOfType<CursorManager>();
        inspectionManager = FindObjectOfType<InspectionManager>();
    }

    void OnMouseEnter()
    {
        cursorManager.SetHoveringInspectable(true);
        isHovered = true;
    }

    void OnMouseExit()
    {
        cursorManager.SetHoveringInspectable(false);
        isHovered = false;
    }

    void OnMouseDown()
    {
        if (!inspectionManager.IsInspecting())
        {
            inspectionManager.StartInspection(transform);
        }
    }

    void Update()
    {
        if (isHovered && Input.GetKeyDown(KeyCode.E))
        {
            if (!inspectionManager.IsInspecting())
            {
                inspectionManager.StartInspection(transform);
            }
        }
    }
}
