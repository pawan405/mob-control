
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 8f;
    public float horizontalLimit = 5f;

    // Shared by all players, including newly spawned players.
    private static bool gameStarted = false;

    private void Update()
    {
        // Start the entire crowd with one click or tap.
        if (!gameStarted)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                gameStarted = true;
                Debug.Log("Game Started!");
            }
            else if (Touchscreen.current != null &&
                     Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                gameStarted = true;
                Debug.Log("Game Started!");
            }
        }

        if (!gameStarted)
            return;

        // Automatically move forward along the Z axis.
        transform.position +=
            Vector3.forward * forwardSpeed * Time.deltaTime;

        float horizontal = 0f;

        // Keyboard controls.
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

        // Mobile horizontal dragging.
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            float deltaX =
                Touchscreen.current.primaryTouch.delta.ReadValue().x;

            horizontal = Mathf.Clamp(deltaX / 100f, -1f, 1f);
        }

        // Move left or right.
        transform.position +=
            Vector3.right * horizontal *
            sideSpeed * Time.deltaTime;

        // Keep the crowd inside the track.
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(
            position.x,
            -horizontalLimit,
            horizontalLimit
        );

        transform.position = position;
    }
}
