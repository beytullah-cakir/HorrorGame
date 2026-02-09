using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimatorController : MonoBehaviour, IInteractable
{
    [Header("Animation Settings")]
    [Tooltip("The name of the boolean parameter in your Animator controller.")]
    [SerializeField] private string animatorBoolName = "IsOpen";
    
    [Header("Highlight Settings")]
    [Tooltip("The material to add as an outline/highlight for this door.")]
    [SerializeField] private Material highlightMaterial;
    [Tooltip("The renderer that should be highlighted.")]
    [SerializeField] private Renderer targetRenderer;

    private Animator _animator;
    private bool _isOpen = false;
    private Material[] _originalMaterials;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        // If no renderer assigned, try to find one
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
    }

    // IInteractable implementation
    public void Interact()
    {
        _isOpen = !_isOpen;
        if (_animator != null) _animator.SetBool(animatorBoolName, _isOpen);
        Debug.Log(_isOpen ? "Kapı Açılıyor" : "Kapı Kapanıyor");
    }

    public void OnHoverEnter()
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

    public void OnHoverExit()
    {
        if (targetRenderer == null || _originalMaterials == null) return;
        targetRenderer.materials = _originalMaterials;
        _originalMaterials = null;
    }
}
