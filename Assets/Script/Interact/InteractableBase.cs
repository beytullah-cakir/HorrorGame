using UnityEngine;

namespace QuestSystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        public abstract void Interact();

        public virtual bool CanInteract()
        {
            return true;
        }
    }
}
