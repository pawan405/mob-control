
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 8f;
    public float horizontalLimit = 3.2f;

    private Rigidbody rb;
    private bool isRunning = false;

    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        Vector3 movement = new Vector3(
            horizontalInput * sideSpeed,
            0f,
            forwardSpeed
        );

        Vector3 newPosition = rb.position +
            movement * Time.fixedDeltaTime;

        // Horizontal boundary
        newPosition.x = Mathf.Clamp(
    newPosition.x,
    -3f,
    3f
);

        rb.MovePosition(newPosition);
    }
}
