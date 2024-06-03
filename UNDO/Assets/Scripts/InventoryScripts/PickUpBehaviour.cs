using UnityEngine;

namespace UNDO
{
    public class PickUpBehaviour : Interactable
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
            if (item.interactText.Length > 0 && !showingMessage)
            {
                showingMessage = true;
                ShowPickUpMessage();
            }

            AddToInventory();
        }

        public override void StopInteraction()
        {
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

        private void AddToInventory()
        {
            Inventory inventory = Inventory.instance;
            if (inventory != null)
            {
                if (inventory.Add(item, quantity))
                {
                    Debug.Log("Item added to inventory: " + item.name);
                    Destroy(gameObject); // Remove the item from the world after adding to inventory
                }
            }
        }
    }
}
