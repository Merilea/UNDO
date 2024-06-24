using UnityEngine;

public class PlacementZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlacementIndicator"))
        {
            other.GetComponent<Renderer>().material.color = Color.green; // Change to green
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementIndicator"))
        {
            other.GetComponent<Renderer>().material.color = Color.white; // Change back to white
        }
    }

    public bool IsWithinBounds(Vector3 position)
    {
        return GetComponent<Collider>().bounds.Contains(position);
    }
}
