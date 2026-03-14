using UnityEngine;

namespace QuestSystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Highlight Settings")]
        [SerializeField] protected Material highlightMaterial;
        [SerializeField] protected Renderer targetRenderer;

        protected Material[] _originalMaterials;

        protected virtual void Awake()
        {
            if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        }

        public abstract void Interact();

        public virtual void OnHoverEnter()
        {
            if (targetRenderer == null || highlightMaterial == null) return;

            _originalMaterials = targetRenderer.sharedMaterials;
            Material[] newMaterials = new Material[_originalMaterials.Length + 1];
            for (int i = 0; i < _originalMaterials.Length; i++)
            {
                newMaterials[i] = _originalMaterials[i];
            }
            newMaterials[newMaterials.Length - 1] = highlightMaterial;
            targetRenderer.materials = newMaterials;
        }

        public virtual void OnHoverExit()
        {
            if (targetRenderer == null || _originalMaterials == null) return;
            targetRenderer.materials = _originalMaterials;
            _originalMaterials = null;
        }
    }
}
