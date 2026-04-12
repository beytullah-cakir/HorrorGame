using UnityEngine;
using UnityEngine.InputSystem;
using DialogueSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Stats")]
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed = 4.0f;
    [Tooltip("Rotation speed for the camera mouse look.")]
    public float rotationSpeed = 1.0f;
    [Tooltip("The force applied when jumping")]
    public float jumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is usually -9.81f")]
    public float gravity = -15.0f;

    [Header("Look Settings")]
    [Tooltip("How far in degrees can you move the camera up")]
    public float topClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float bottomClamp = -90.0f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject cinemachineCameraTarget;

    [Header("Footsteps")]
    [Tooltip("The audio source that will play footstep sounds")]
    public AudioSource footstepAudioSource;
    [Tooltip("How often to play a footstep sound while walking")]
    public float footstepFrequency = 0.5f;

    // Internal variables
    private CharacterController _controller;
    private PlayerInputSystem _inputSystem;
    private float _cinemachineTargetPitch;
    private float _rotationVelocity;
    private Vector3 _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private float _footstepTimer = 0.0f;

    // Input values
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumpInput;
    private int _lockCount = 0;
    public bool isTimelinePlaying = false;
    private bool IsLocked => _lockCount > 0 || isTimelinePlaying;

    public void SetTimelineState(bool active)
    {
        isTimelinePlaying = active;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputSystem = new PlayerInputSystem();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _inputSystem.Player.Enable();
        DialogueManager.OnDialogueStarted += LockPlayer;
        DialogueManager.OnDialogueFinished += UnlockPlayer;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Disable();
        DialogueManager.OnDialogueStarted -= LockPlayer;
        DialogueManager.OnDialogueFinished -= UnlockPlayer;
    }

    private void Update()
    {
        ReadInput();
        GroundCheck();
        JumpAndGravity();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void ReadInput()
    {
        if (isTimelinePlaying)
        {
            _moveInput = Vector2.zero;
            _lookInput = Vector2.zero;
            _jumpInput = false;
            return;
        }

        // Always read look input so the player can look around even if move is locked (during dialogues)
        _lookInput = _inputSystem.Player.Look.ReadValue<Vector2>();

        if (IsLocked)
        {
            _moveInput = Vector2.zero;
            _jumpInput = false;
            return;
        }

        // Read movement input values directly from the generated class instance
        _moveInput = _inputSystem.Player.Move.ReadValue<Vector2>();
        _jumpInput = _inputSystem.Player.Jump.WasPressedThisFrame();
    }

    public void LockPlayer() { _lockCount++; }
    public void UnlockPlayer() { _lockCount = Mathf.Max(0, _lockCount - 1); }

    private void GroundCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_lookInput.sqrMagnitude >= 0.01f)
        {
            // Update rotation values directly from input
            // Mouse input is already frame-rate independent usually, but scaling helps control sensitivity
            float deltaTimeMultiplier = 1.0f; 
            
            _cinemachineTargetPitch += _lookInput.y * rotationSpeed * deltaTimeMultiplier * -1.0f;
            _rotationVelocity = _lookInput.x * rotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            // Update Cinemachine camera target pitch
            if (cinemachineCameraTarget != null)
            {
                cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            }
            
            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private void Move()
    {
        // set target speed based on move speed
        float targetSpeed = moveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_moveInput == Vector2.zero) targetSpeed = 0.0f;

        // move the player
        Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_moveInput != Vector2.zero)
        {
            // move
            inputDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        }

        // move the player
        _controller.Move(inputDirection.normalized * (targetSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity.y, 0.0f) * Time.deltaTime);

        HandleFootsteps(targetSpeed);
    }

    private void HandleFootsteps(float speed)
    {
        if (!grounded || _moveInput == Vector2.zero) return;

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0)
        {
            if (footstepAudioSource != null && footstepAudioSource.clip != null)
            {
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
            }

            _footstepTimer = footstepFrequency;
        }
    }

    private void JumpAndGravity()
    {
        if (grounded)
        {
            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity.y < 0.0f)
            {
                _verticalVelocity.y = -2f;
            }

            // Jump
            if (_jumpInput)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity.y < _terminalVelocity)
        {
            _verticalVelocity.y += gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }
}
