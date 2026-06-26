using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;

    public Transform cam;

    public float jumpForce = 6f;

    Rigidbody rb;

    float xRotation;

    float moveX;
    float moveZ;

    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState =
            CursorLockMode.Locked;
    }

    void Update()
    {
        moveX =
            Input.GetAxisRaw(
                "Horizontal"
            );

        moveZ =
            Input.GetAxisRaw(
                "Vertical"
            );

        float mouseX =
            Input.GetAxis(
                "Mouse X"
            )
            *
            mouseSensitivity
            *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis(
                "Mouse Y"
            )
            *
            mouseSensitivity
            *
            Time.deltaTime;

        xRotation -= mouseY;

        xRotation =
            Mathf.Clamp(
                xRotation,
                -80,
                80
            );

        cam.localRotation =
            Quaternion.Euler(
                xRotation,
                0,
                0
            );

        transform.Rotate(
            Vector3.up
            *
            mouseX
        );

        // JUMP
        if (
            Input.GetKeyDown(
                KeyCode.Space
            )
            &&
            isGrounded
        )
        {
            rb.AddForce(
                Vector3.up
                *
                jumpForce,
                ForceMode.Impulse
            );
        }
    }

    void FixedUpdate()
    {
        Vector3 move =
            (
                transform.right
                *
                moveX
                +
                transform.forward
                *
                moveZ
            )
            *
            moveSpeed;

        rb.linearVelocity =
            new Vector3(
                move.x,
                rb.linearVelocity.y,
                move.z
            );

        GroundCheck();
    }

    void GroundCheck()
    {
        isGrounded =
            Physics.Raycast(
                transform.position,
                Vector3.down,
                1.2f
            );
    }
}