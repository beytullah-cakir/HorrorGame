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
        if (_inputSystem == null) _inputSystem = new PlayerInputSystem();
        _inputSystem.Player.Enable(); 
    }

    private void OnDisable()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Player.Disable();
        }
        
        if (_currentInteractable != null)
        {
            _currentInteractable.OnHoverExit();
            _currentInteractable = null;
        }
    }

    private void Update()
    {
        CheckForInteractable();

        // E tuşuna basılıp basılmadığını Update içinde kontrol et
        if (_inputSystem.Player.Interact.WasPressedThisFrame())
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }
    }

    private void CheckForInteractable()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green); // Görsel test için
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactable != _currentInteractable)
                {
                    if (_currentInteractable != null) _currentInteractable.OnHoverExit();
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
}
