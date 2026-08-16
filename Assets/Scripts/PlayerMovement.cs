using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Animator animator;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 8f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Control")]
    public bool canMove = true;

    private Vector3 velocity;

    void Update()
    {
        ApplyGravity();

        if (!canMove)
        {
            // Keep player grounded while movement is locked
            animator.SetFloat("Speed", 0f);
            return;
        }

        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? runSpeed
            : walkSpeed;

        controller.Move(
            move * speed * Time.deltaTime
        );

        // Animation
        float animationSpeed = move.magnitude;

        if (Input.GetKey(KeyCode.LeftShift) &&
            animationSpeed > 0)
        {
            animationSpeed = 1f;
        }
        else
        {
            animationSpeed *= 0.5f;
        }

        animator.SetFloat(
            "Speed",
            animationSpeed
        );
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }
}