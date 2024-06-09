using System.Collections.Generic;
using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    public static GameTaskManager Instance;

    private List<GameTask> tasks = new List<GameTask>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterTask(GameTask task)
    {
        tasks.Add(task);
    }

    public void CompleteTask(GameTask task)
    {
        task.CompleteTask();
    }
}
