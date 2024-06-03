using UNDO;
using UnityEngine;

namespace UNDO
{
    public class ItemPickupBehavior : Interactable
    {
        public ItemSO item;
        [SerializeField] public int quantity = 1;
        private bool showingMessage = false;
        private InteractionMessageBehavior messageUI;

        private void Start()
        {
            messageUI = InteractionMessageBehavior.instance;
        }

        public override void Interact()
        {
            base.Interact();
            if (item.interactText.Length > 0 && !showingMessage)
            {
                showingMessage = true;
                ShowPickUpMessage();
            }
        }

        public override void StopInteraction()
        {
            base.StopInteraction();
            if (item.interactText.Length > 0)
            {
                TurnOffPickUpMessage();
                showingMessage = false;
            }
        }

        private void TurnOffPickUpMessage()
        {
            messageUI.DeactiveText(MessageType.Interact);
        }

        private void ShowPickUpMessage()
        {
            Debug.Log(item.interactText);
            messageUI.SetText(item.interactText, MessageType.Interact);
        }

        public void PickupItem()
        {
            if (Inventory.instance.Add(item, quantity))
            {
                Debug.Log("Adding " + item.name + " to the inventory");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Failed to add " + item.name + " to the inventory");
            }
        }
    }
}
