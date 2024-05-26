using UnityEngine;

public class PollutionHotspot : Interactable
{
    public float pollutionReductionAmount = 5f;

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.ReducePollution(pollutionReductionAmount);
        Destroy(gameObject); // Remove the placeholder after interaction
    }
}
