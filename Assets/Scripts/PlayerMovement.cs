using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;

    [Header("Camera")]
    public Transform cam;

    [Header("Jump")]
    public float jumpForce = 6f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;

    private float xRotation;

    private float moveX;
    private float moveZ;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ==========================
        // MOVEMENT INPUT
        // ==========================

        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");


        // ==========================
        // MOUSE LOOK
        // ==========================

        float mouseX =
            Input.GetAxis("Mouse X")
            * mouseSensitivity
            * Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y")
            * mouseSensitivity
            * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(
            xRotation,
            -80f,
            80f
        );

        cam.localRotation =
            Quaternion.Euler(
                xRotation,
                0f,
                0f
            );

        transform.Rotate(
            Vector3.up * mouseX
        );


        // ==========================
        // JUMP
        // ==========================

        if (
            Input.GetKeyDown(KeyCode.Space)
            &&
            isGrounded
        )
        {
            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }
    }

    void FixedUpdate()
    {
        // ==========================
        // MOVEMENT
        // ==========================

        Vector3 move =
            (
                transform.right * moveX
                +
                transform.forward * moveZ
            ).normalized * moveSpeed;

        rb.linearVelocity =
            new Vector3(
                move.x,
                rb.linearVelocity.y,
                move.z
            );


        // ==========================
        // GROUND CHECK
        // ==========================

        GroundCheck();
    }

    void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}