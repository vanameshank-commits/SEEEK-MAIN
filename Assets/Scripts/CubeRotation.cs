using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationAmount = 90f;

    [Header("Lift")]
    public float liftAmount = 0.25f;

    private bool rotating = false;
    private Vector3 deskPosition;

    public void StartRotation()
    {
        if (rotating)
            return;

        rotating = true;

        deskPosition = transform.position;

        // Lift cube slightly
        transform.position =
            deskPosition + Vector3.up * liftAmount;
    }

    void Update()
    {
        if (!rotating)
            return;

        // 1 = rotate X -
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.Rotate(
                Vector3.right,
                -rotationAmount,
                Space.Self
            );
        }

        // 2 = rotate X +
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.Rotate(
                Vector3.right,
                rotationAmount,
                Space.Self
            );
        }

        // 3 = rotate Y -
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.Rotate(
                Vector3.up,
                -rotationAmount,
                Space.Self
            );
        }

        // 4 = rotate Y +
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.Rotate(
                Vector3.up,
                rotationAmount,
                Space.Self
            );
        }
    }

    public void StopRotation()
    {
        rotating = false;
    }
}