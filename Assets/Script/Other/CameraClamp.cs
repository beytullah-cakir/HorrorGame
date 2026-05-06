using UnityEngine;
using UnityEngine.InputSystem;

public class CameraClamp : MonoBehaviour
{
    [Header("Rotation Limits")]
    [SerializeField] private float minPitch = -90f;
    [SerializeField] private float maxPitch = 90f;
    
    [Header("Sensitivity")]
    [SerializeField] private float sensitivity = 0.1f; // New Input System delta values are larger

    private float _currentPitch = 0f;

    private void Start()
    {
        _currentPitch = transform.localEulerAngles.x;
        if (_currentPitch > 180) _currentPitch -= 360;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        // Mouse delta Y değerini oku
        float mouseY = Mouse.current.delta.y.ReadValue() * sensitivity;

        _currentPitch -= mouseY;
        _currentPitch = Mathf.Clamp(_currentPitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(_currentPitch, transform.localRotation.eulerAngles.y, 0f);
    }
}
