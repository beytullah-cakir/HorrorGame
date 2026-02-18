using UnityEngine;

public class DoorAnimatorController : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [Tooltip("The angle to rotate (y-axis) when the door is open.")]
    public float openRotationAngle = 90f;
    [Tooltip("How fast the door rotates.")]
    [SerializeField] private float rotationSpeed = 5f;
    [Tooltip("Is the door currently locked?")]
    public bool isLocked = false;
    [Tooltip("Check this if the door opens towards the player instead of away.")]
    public bool invertRotation = false;
    
    [Header("Highlight Settings")]
    [Tooltip("The material to add as an outline/highlight for this door.")]
    [SerializeField] private Material highlightMaterial;
    [Tooltip("The renderer that should be highlighted.")]
    [SerializeField] private Renderer targetRenderer;

    private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _targetRotation;
    private Material[] _originalMaterials;

    private void Awake()
    {
        _closedRotation = transform.localRotation;
        _targetRotation = _closedRotation;
        
        // If no renderer assigned, try to find one
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, Time.deltaTime * rotationSpeed);
    }

    // IInteractable implementation
    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("Kapı kilitli, açılamıyor!");
            return;
        }

        if (!_isOpen)
        {
            // Get player position relative to the door's local space
            Vector3 localPlayerPos = transform.InverseTransformPoint(Camera.main.transform.position);
            
            // If localPlayerPos.z is positive, player is in front of the door's Z axis
            // We flip the rotation to open AWAY from the player
            float rotationSign = localPlayerPos.z >= 0 ? 1f : -1f;
            
            // Apply inversion if needed based on model axes
            if (invertRotation) rotationSign *= -1f;

            _targetRotation = _closedRotation * Quaternion.Euler(0, openRotationAngle * rotationSign, 0);
            _isOpen = true;
            Debug.Log("Kapı Açılıyor");
        }
        else
        {
            _targetRotation = _closedRotation;
            _isOpen = false;
            Debug.Log("Kapı Kapanıyor");
        }
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
