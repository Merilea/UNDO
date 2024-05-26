using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * interactRange, Color.green, 2.0f);

        if (Physics.Raycast(transform.position, forward, out hit, interactRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("Interacted with: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Hit non-interactable object: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("No interactable object hit");
        }
    }
}
