using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody; // Reference to the player's body for horizontal rotation

    [Header("Settings")]
    public float mouseSensitivity = 1f;
    public bool lockCursor = true;

    private float xRotation = 0f;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        xRotation = transform.localEulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * 0.1f;

        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        // Vertical rotation (camera up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body left/right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
