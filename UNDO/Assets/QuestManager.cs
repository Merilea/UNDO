using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public TextMeshProUGUI questText;
    private Queue<GameTask> tasksQueue = new Queue<GameTask>();
    private GameTask currentTask;

    public void RegisterTask(GameTask task)
    {
        tasksQueue.Enqueue(task);
        if (currentTask == null)
        {
            StartNextTask();
        }
    }

    public void CompleteCurrentTask()
    {
        if (currentTask != null)
        {
            currentTask.isCompleted = true;
            currentTask.gameObject.SetActive(false);
            StartNextTask();
        }
    }

    private void StartNextTask()
    {
        if (tasksQueue.Count > 0)
        {
            currentTask = tasksQueue.Dequeue();
            currentTask.gameObject.SetActive(true);
            currentTask.OnTaskStart();
            UpdateQuestText(currentTask.taskDescription);
        }
        else
        {
            currentTask = null;
            UpdateQuestText("All tasks completed!");
        }
    }

    private void UpdateQuestText(string text)
    {
        if (questText != null)
        {
            questText.text = text;
        }
    }
}
