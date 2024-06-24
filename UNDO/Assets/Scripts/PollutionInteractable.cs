using UnityEngine;
using TMPro;

namespace UNDO
{
    public class PollutionInteractable : Interactable
    {
        public TextMeshProUGUI turnOffText;
        [TextArea] // This attribute makes the text field in the Inspector larger
        public string interactionMessage = "Press E to Turn Off";
        public float pollutionDecreaseAmount = 10f;
        private bool isInteractable = true;

        private void Start()
        {
            if (turnOffText != null)
            {
                turnOffText.gameObject.SetActive(false);
            }
        }

        public override void Interact()
        {
            if (isInteractable)
            {
                PollutionLevel.Instance.DecreasePollution(pollutionDecreaseAmount);
                isInteractable = false;
                HideTurnOffText();
            }
        }

        public void ShowTurnOffText(Vector3 screenPosition)
        {
            if (isInteractable && turnOffText != null)
            {
                turnOffText.gameObject.SetActive(true);
                turnOffText.transform.position = screenPosition;
                turnOffText.text = interactionMessage;
            }
        }

        public override void StopInteraction()
        {
            HideTurnOffText();
        }

        private void HideTurnOffText()
        {
            if (turnOffText != null)
            {
                turnOffText.gameObject.SetActive(false);
            }
        }
    }
}
