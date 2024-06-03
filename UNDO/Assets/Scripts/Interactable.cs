using UnityEngine;

namespace UNDO
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact();
        public virtual void StopInteraction() { }
    }
}
