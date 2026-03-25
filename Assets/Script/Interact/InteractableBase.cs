using UnityEngine;

namespace QuestSystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Highlight Settings (DEPRECATED)")]
        [SerializeField] protected Renderer targetRenderer;
        protected Material[] _originalMaterials;

        protected virtual void Awake()
        {
            if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        }

        public abstract void Interact();

        public virtual void OnHoverEnter()
        {
            // Hover highlight logic removed
        }

        public virtual void OnHoverExit()
        {
            // Hover highlight logic removed
        }

        public virtual bool CanInteract()
        {
            return true;
        }
    }
}
