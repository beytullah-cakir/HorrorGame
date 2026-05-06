using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController Instance;
    public GameObject flashlightObject;

    
    [SerializeField] private bool _hasFlashlight = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
       
        if (!_hasFlashlight) return;

       
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        if (flashlightObject == null) return;

        bool nextState = !flashlightObject.activeSelf;
        flashlightObject.SetActive(nextState);
    }

   
    public void EnableFlashlight()
    {
        _hasFlashlight = true;
        if (flashlightObject != null) flashlightObject.SetActive(true);
    }
}
