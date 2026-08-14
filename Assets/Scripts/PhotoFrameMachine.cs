using UnityEngine;

public class PhotoFrameMachine : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject eImage;

    [Header("Settings")]
    public float interactDistance = 3f;

    private Rigidbody rb;
    private bool fallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (eImage != null)
            eImage.SetActive(false);
    }

    void Update()
    {
        if (fallen)
        {
            if (eImage != null)
                eImage.SetActive(false);

            return;
        }

        bool lookingAtFrame = IsLookingAtFrame();

        // E image
        if (eImage != null)
            eImage.SetActive(lookingAtFrame);

        // Press E
        if (lookingAtFrame &&
            Input.GetKeyDown(KeyCode.E))
        {
            FallFrame();
        }
    }

    bool IsLookingAtFrame()
    {
        if (playerCamera == null)
            return false;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    void FallFrame()
    {
        fallen = true;

        if (eImage != null)
            eImage.SetActive(false);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Debug.Log("Photo frame fell.");
    }
}