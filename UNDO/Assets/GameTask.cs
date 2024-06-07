using UnityEngine;

public class GameTask : MonoBehaviour
{
    public string taskDescription;
    public bool isCompleted;

    public virtual void OnTaskStart()
    {
        Debug.Log("Task started: " + taskDescription);
    }

    public virtual void OnTaskComplete()
    {
        Debug.Log("Task completed: " + taskDescription);
    }

    public void CompleteTask()
    {
        isCompleted = true;
        OnTaskComplete();
    }
}
