using UnityEngine;
using QuestSystem;

public class DrawerController : InteractableBase
{
    public enum MoveAxis { X, Y, Z }

    [Header("Movement Settings")]
    [Tooltip("The axis along which the drawer slides.")]
    [SerializeField] private MoveAxis movementAxis = MoveAxis.Z;
    [Tooltip("How far the drawer slides.")]
    [SerializeField] private float openDistance = 0.5f;
    [Tooltip("How fast the drawer slides.")]
    [SerializeField] private float slideSpeed = 5f;

    [Header("Quest Requirements")]
    [Tooltip("The quest that must be active to interact with this drawer.")]
    [SerializeField] private Quest requiredQuest;
    [Tooltip("If true, the drawer can be interacted with even if no quest is active (for testing or general use).")]
    [SerializeField] private bool ignoreQuestRequirement = false;

    private bool _isOpen = false;
    private Vector3 _closedPosition;
    private Vector3 _targetPosition;

    protected override void Awake()
    {
        base.Awake();
        _closedPosition = transform.localPosition;
        _targetPosition = _closedPosition;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * slideSpeed);
    }

    public override bool CanInteract()
    {
        if (ignoreQuestRequirement || requiredQuest == null)
        {
            return true;
        }

        Quest activeQuest = QuestManager.Instance.GetActiveQuest();
        return activeQuest == requiredQuest;
    }

    public override void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        if (!_isOpen)
        {
            Vector3 offset = Vector3.zero;
            switch (movementAxis)
            {
                case MoveAxis.X: offset = new Vector3(openDistance, 0, 0); break;
                case MoveAxis.Y: offset = new Vector3(0, openDistance, 0); break;
                case MoveAxis.Z: offset = new Vector3(0, 0, openDistance); break;
            }
            _targetPosition = _closedPosition + offset;
            _isOpen = true;
        }
        else
        {
            _targetPosition = _closedPosition;
            _isOpen = false;
        }
    }
}
