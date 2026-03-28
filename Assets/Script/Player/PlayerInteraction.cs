using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    
    private Camera playerCamera;
    [Tooltip("The max distance to interact with objects.")]
    public float interactionDistance = 3.0f;
    
    [Tooltip("The crosshair RectTransform in the center of the screen.")]
    public RectTransform crosshairRect;

    [Header("Crosshair Settings")]
    public float normalScale = 0.5f;
    public float hoverScale = 1.0f;
    public float scaleSpeed = 10.0f;

    private Vector3 _currentCrosshairScale;

    private PlayerInputSystem _inputSystem;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _inputSystem = new PlayerInputSystem();
        if (crosshairRect != null) _currentCrosshairScale = Vector3.one * normalScale;
        playerCamera = Camera.main;
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
            
            _currentInteractable = null;
        }
    }

    private void Update()
    {
        CheckForInteractable();
        HandleCrosshairScale();

        // E tuşuna basılıp basılmadığını Update içinde kontrol et
        if (_inputSystem.Player.Interact.WasPressedThisFrame())
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }
    }

    private void HandleCrosshairScale()
    {
        if (crosshairRect == null) return;

        float targetScaleValue = (_currentInteractable != null && _currentInteractable.CanInteract()) ? hoverScale : normalScale;
        Vector3 targetScale = Vector3.one * targetScaleValue;

        _currentCrosshairScale = Vector3.Lerp(_currentCrosshairScale, targetScale, Time.deltaTime * scaleSpeed);
        crosshairRect.localScale = _currentCrosshairScale;
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
                    _currentInteractable = interactable;
                }
                
                
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
            
            _currentInteractable = null;
        }
    }
}
