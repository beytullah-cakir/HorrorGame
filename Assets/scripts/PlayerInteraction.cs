using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("The camera used for raycasting. Usually Main Camera.")]
    public Camera playerCamera;
    [Tooltip("The max distance to interact with objects.")]
    public float interactionDistance = 3.0f;
    [Tooltip("The UI GameObject showing the 'E' prompt.")]
    public GameObject interactionPromptUI;

    private PlayerInputSystem _inputSystem;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _inputSystem = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Interact.Enable();
        _inputSystem.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Interact.performed -= OnInteractPerformed;
        _inputSystem.Player.Interact.Disable();
        
        if (_currentInteractable != null)
        {
            _currentInteractable.OnHoverExit();
            _currentInteractable = null;
        }
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactable != _currentInteractable)
                {
                    // Exit previous
                    if (_currentInteractable != null) _currentInteractable.OnHoverExit();
                    
                    // Enter new
                    _currentInteractable = interactable;
                    _currentInteractable.OnHoverEnter();
                }
                
                if (interactionPromptUI != null) interactionPromptUI.SetActive(true);
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.OnHoverExit();
            _currentInteractable = null;
        }

        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }
}
