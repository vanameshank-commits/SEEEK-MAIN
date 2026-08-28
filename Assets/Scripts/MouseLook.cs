using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;

    public float mouseSensitivity = 2f;

    public float minLookAngle = -5f;
    public float maxLookAngle = 30f;

    public bool canLook = true;

    private float xRotation = 0f;

    void Update()
    {
        // Stop ONLY camera movement while pause menu is open
        if (PauseMenu.IsPaused)
            return;

        if (!canLook)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(
            xRotation,
            minLookAngle,
            maxLookAngle
        );

        transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}