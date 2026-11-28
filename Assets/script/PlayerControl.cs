
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 8f;
    public float horizontalLimit = 3.2f;
    public bool followSpawnHeading;
    public bool startAutomatically;

    private Rigidbody rb;
    private bool isRunning = false;

    private float horizontalInput;
    private Vector3 startPosition;
    private Vector3 forward;
    private Vector3 right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = rb.position;
        forward = followSpawnHeading
            ? Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized
            : Vector3.forward;
        right = Vector3.Cross(Vector3.up, forward);
        isRunning = startAutomatically;
    }

    private void Update()
    {
        // Start game
        if (!isRunning)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                isRunning = true;
            }
            else if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                isRunning = true;
            }
        }

        if (!isRunning)
        {
            horizontalInput = 0f;
            return;
        }

        float horizontal = 0f;

        // Keyboard movement
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal -= 1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal += 1f;
            }
        }

        // Mobile drag movement
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            float deltaX =
                Touchscreen.current.primaryTouch.delta.ReadValue().x;

            horizontal = Mathf.Clamp(deltaX / 100f, -1f, 1f);
        }

        horizontalInput = horizontal;
    }

    private void FixedUpdate()
    {
        if (!isRunning)
            return ;

        Vector3 movement = forward * forwardSpeed + right * (horizontalInput * sideSpeed);

        Vector3 newPosition = rb.position +
            movement * Time.fixedDeltaTime;

        // Horizontal boundary
        float lateral = Vector3.Dot(newPosition - startPosition, right);
        newPosition += right * (Mathf.Clamp(lateral, -horizontalLimit, horizontalLimit) - lateral);

        rb.MovePosition(newPosition);
    }
}
