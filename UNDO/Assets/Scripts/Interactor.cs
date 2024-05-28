using UNDO;
using UnityEngine;

namespace UNDO
{
    public class Interactor : MonoBehaviour
    {
        public float interactRange = 3f;
        public LayerMask interactableLayer;

        private Interactable currentInteractable;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractable != null)
                {
                    ItemPickupBehavior itemPickup = currentInteractable.GetComponent<ItemPickupBehavior>();
                    if (itemPickup != null)
                    {
                        itemPickup.PickupItem();
                    }
                }
            }

            CheckForInteractable();
        }

        void CheckForInteractable()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, forward * interactRange, Color.green, 2.0f);

            if (Physics.Raycast(transform.position, forward, out hit, interactRange, interactableLayer))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (currentInteractable != interactable)
                    {
                        if (currentInteractable != null)
                        {
                            currentInteractable.StopInteraction();
                        }
                        currentInteractable = interactable;
                        currentInteractable.Interact();
                    }
                }
                else
                {
                    if (currentInteractable != null)
                    {
                        currentInteractable.StopInteraction();
                        currentInteractable = null;
                    }
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.StopInteraction();
                    currentInteractable = null;
                }
            }
        }
    }
}
