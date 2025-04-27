using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class AimStateManager : MonoBehaviour
{
    [SerializeField] Transform camFollowPos;
    [SerializeField] float mouseSensitivity = 1f;

    float xRotation;
    float yRotation;
    PlayerInput playerInput;
    InputAction lookAction;

    void Start()
    {
        // Set up input
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            lookAction = playerInput.actions["Look"];
            lookAction?.Enable();
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        lookAction?.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (lookAction == null)
        {
            Debug.LogWarning("lookAction is NULL!");
        }
        else
        {
            Vector2 lookDelta = lookAction.ReadValue<Vector2>();
            Debug.Log("LookDelta: " + lookDelta);
        }

        if (lookAction != null)
        {
            // Get input from new Input System
            Vector2 lookDelta = lookAction.ReadValue<Vector2>();

            // Apply sensitivity
            xRotation += lookDelta.x * mouseSensitivity;
            yRotation -= lookDelta.y * mouseSensitivity;

            // Clamp vertical rotation
            yRotation = Mathf.Clamp(yRotation, -80f, 80f);
        }
    }

    void LateUpdate()
    {
        if (camFollowPos != null)
        {
            // Apply rotations
            camFollowPos.localEulerAngles = new Vector3(yRotation, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xRotation, transform.eulerAngles.z);
        }
    }
}