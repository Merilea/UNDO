using UnityEngine;

public class SolarPanelTask : MonoBehaviour
{
    private bool isTaskStarted = false;

    private void Start()
    {
        if (!isTaskStarted)
        {
            StartTask();
            isTaskStarted = true;
        }
    }

    public void StartTask()
    {
        Debug.Log("Solar Panel Task Started");
        // You can add additional initialization code here if needed
    }

    public void CheckPlacement(Vector3 position, CleanEnergyStation station)
    {
        TaskManager.Instance.ShowTaskButtons(station);
    }
}
