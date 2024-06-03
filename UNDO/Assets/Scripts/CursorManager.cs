using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D inspectCursor;
    private bool isHoveringInspectable;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (isHoveringInspectable)
        {
            Cursor.SetCursor(inspectCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void SetHoveringInspectable(bool isHovering)
    {
        isHoveringInspectable = isHovering;
    }
}
