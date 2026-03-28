using UnityEngine;
using QuestSystem;

public class DoorAnimatorController : InteractableBase
{
    [Header("Rotation Settings")]
    [Tooltip("The angle to rotate (y-axis) when the door is open.")]
    public float openRotationAngle = 90f;
    [Tooltip("How fast the door rotates.")]
    [SerializeField] private float rotationSpeed = 5f;
    [Tooltip("Is the door currently locked?")]
    public bool isLocked = false;
    
    
    private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _targetRotation;

    protected void Awake()
    {
        
        _closedRotation = transform.localRotation;
        _targetRotation = _closedRotation;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, Time.deltaTime * rotationSpeed);
    }

    // InteractableBase (IInteractable) implementation
    public override void Interact()
    {
        if (isLocked)
        {
            return;
        }

        if (!_isOpen)
        {
            // Get player position relative to the door's local space
            Vector3 localPlayerPos = transform.InverseTransformPoint(Camera.main.transform.position);
            
            // If localPlayerPos.z is positive, player is in front of the door's Z axis
            // We flip the rotation to open AWAY from the player
            float rotationSign = localPlayerPos.z >= 0 ? 1f : -1f;
           

            _targetRotation = _closedRotation * Quaternion.Euler(0, openRotationAngle * rotationSign, 0);
            _isOpen = true;
        }
        else
        {
            _targetRotation = _closedRotation;
            _isOpen = false;
        }
    }
}
