using UnityEngine;

public class PollutionHotspot : Interactable
{
    public float pollutionReductionAmount = 5f;

    public override void Interact()
    {
        // Remove the base.Interact() call as it's abstract and cannot be called
        GameManager.Instance.ReducePollution(pollutionReductionAmount);
        Destroy(gameObject); // Remove the placeholder after interaction
    }
}
