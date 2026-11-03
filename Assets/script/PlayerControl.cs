
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 8f;
    public float horizontalLimit = 5f;

    private bool isRunning = false;

    private void Update()
    {
        // Start crowd with one click or tap.
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
            return;

        // Automatic forward movement.
        transform.position +=
            Vector3.forward * forwardSpeed * Time.deltaTime;

        float horizontal = 0f;

        // Keyboard movement.
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
                horizontal -= 1f;

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
                horizontal += 1f;
        }

        // Mobile drag movement.
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            float deltaX =
                Touchscreen.current.primaryTouch.delta.ReadValue().x;

            horizontal = Mathf.Clamp(deltaX / 100f, -1f, 1f);
        }

        // Move entire crowd left/right.
        transform.position +=
            Vector3.right * horizontal *
            sideSpeed * Time.deltaTime;

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(
            position.x,
            -horizontalLimit,
            horizontalLimit
        );

        transform.position = position;
    }
}
