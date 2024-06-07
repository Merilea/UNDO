using UnityEngine;

public class SolarPanelTask : GameTask
{
    public Collider[] validPlacementZones; // Array of valid placement zones

    private void Start()
    {
        QuestManager.instance.RegisterTask(this);
    }

    public override void OnTaskStart()
    {
        Debug.Log("Solar Panel Task Started");
    }

    public override void OnTaskComplete()
    {
        Debug.Log("Solar Panel Task Completed");
        QuestManager.instance.CompleteCurrentTask();
    }

    public void OnPlacementComplete(GameObject placedItem)
    {
        foreach (Collider zone in validPlacementZones)
        {
            if (zone.bounds.Contains(placedItem.transform.position))
            {
                CompleteTask();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlacementIndicator"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementIndicator"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white; // or whatever the default color is
            }
        }
    }
}
