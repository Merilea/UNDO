using UNDO;
using UnityEngine;

public abstract class GameTask : MonoBehaviour
{
    protected virtual void Start()
    {
        GameTaskManager.Instance.RegisterTask(this);
    }

    public abstract void StartTask();
    public abstract void CompleteTask();
    public abstract void CheckPlacement(Vector3 position, CleanEnergyStation cleanEnergyStation);
}
