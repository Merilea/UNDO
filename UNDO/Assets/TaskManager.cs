using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public Canvas taskButtonsCanvas;
    public Button doneButton;
    public Button notDoneButton;
    public PollutionLevel pollutionLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            taskButtonsCanvas.gameObject.SetActive(false); // Initially toggle off the canvas
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTaskButtons(CleanEnergyStation station)
    {
        taskButtonsCanvas.gameObject.SetActive(true);

        doneButton.onClick.RemoveAllListeners();
        doneButton.onClick.AddListener(() => OnDone(station));

        notDoneButton.onClick.RemoveAllListeners();
        notDoneButton.onClick.AddListener(OnNotDone);
    }

    private void OnDone(CleanEnergyStation station)
    {
        if (station != null)
        {
            pollutionLevel.DecreasePollution(10);
            station.SetUninteractable();
        }
        HideTaskButtons();
    }


    private void OnNotDone()
    {
        HideTaskButtons();
    }

    public void HideTaskButtons()
    {
        taskButtonsCanvas.gameObject.SetActive(false);
    }
}
