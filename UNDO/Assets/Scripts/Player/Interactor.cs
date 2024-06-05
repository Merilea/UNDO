using UNDO;
using UnityEngine;
using TMPro;

namespace UNDO
{
    public class Interactor : MonoBehaviour
    {
        public float interactRange = 3f;
        public LayerMask interactableLayer;
        public TextMeshProUGUI pickupText; // Reference to the UI Text element

        private Interactable currentInteractable;
        private float textTimeout = 0.2f; // Timeout period for hiding the text
        private float textTimer;

        private void Start()
        {
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("PickupText reference is not set in the Inspector.");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractable != null)
                {
                    currentInteractable.Interact();
                }
            }

            CheckForInteractable();

            // Hide text if timeout period has passed
            if (pickupText.gameObject.activeSelf && textTimer <= 0)
            {
                HidePickupText();
            }
            else if (pickupText.gameObject.activeSelf)
            {
                textTimer -= Time.deltaTime;
            }
        }

        void CheckForInteractable()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, forward * interactRange, Color.green, 0.1f);

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
                    }
                    ShowPickupText(hit);
                }
                else
                {
                    ClearCurrentInteractable();
                }
            }
            else
            {
                ClearCurrentInteractable();
            }
        }

        void ShowPickupText(RaycastHit hit)
        {
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(true);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);
                pickupText.transform.position = screenPosition;
                textTimer = textTimeout; // Reset the timer
            }
        }

        void HidePickupText()
        {
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(false);
            }
        }

        void ClearCurrentInteractable()
        {
            if (currentInteractable != null)
            {
                currentInteractable.StopInteraction();
                currentInteractable = null;
            }
        }
    }
}
