using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform objectHoldPoint;

    [Header("First Person Body")]
    public GameObject ch01Body;

    [Header("Settings")]
    public float interactDistance = 3f;
    public float dropDistance = 1.5f;

    [Header("Pickup E Images")]
    public GameObject pickupEImage1;
    public GameObject pickupEImage2;
    public GameObject pickupEImage3;
    public GameObject pickupEImage4;

    [Header("Locker UI")]
    public GameObject lockerEImage;

    private GameObject heldItem;

    // Save original world scale
    private Vector3 originalWorldScale;


    void Update()
    {
        UpdatePickupUI();
        UpdateLockerUI();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }


    // =========================================================
    // HIDE PICKUP E IMAGES
    // =========================================================

    void HideAllPickupEImages()
    {
        if (pickupEImage1 != null)
            pickupEImage1.SetActive(false);

        if (pickupEImage2 != null)
            pickupEImage2.SetActive(false);

        if (pickupEImage3 != null)
            pickupEImage3.SetActive(false);

        if (pickupEImage4 != null)
            pickupEImage4.SetActive(false);
    }


    // =========================================================
    // PICKUP E IMAGE
    // =========================================================

    void UpdatePickupUI()
    {
        HideAllPickupEImages();

        if (playerCamera == null)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            if (!hit.collider.CompareTag("PickupObject"))
                return;

            if (hit.collider.gameObject.name == "Cube")
            {
                if (pickupEImage1 != null)
                    pickupEImage1.SetActive(true);
            }
            else if (hit.collider.gameObject.name == "Cube (1)")
            {
                if (pickupEImage2 != null)
                    pickupEImage2.SetActive(true);
            }
            else if (hit.collider.gameObject.name == "Cube (2)")
            {
                if (pickupEImage3 != null)
                    pickupEImage3.SetActive(true);
            }
            else if (hit.collider.gameObject.name == "Cube (3)")
            {
                if (pickupEImage4 != null)
                    pickupEImage4.SetActive(true);
            }
        }
    }


    // =========================================================
    // LOCKER E IMAGE
    // =========================================================

    void UpdateLockerUI()
    {
        if (lockerEImage == null || playerCamera == null)
            return;

        lockerEImage.SetActive(false);

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            Locker locker =
                hit.collider.GetComponentInParent<Locker>();

            if (locker != null)
            {
                lockerEImage.SetActive(true);
            }
        }
    }


    // =========================================================
    // INTERACTION
    // =========================================================

    void TryPickup()
    {
        if (heldItem != null)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            return;
        }


        // =====================================================
        // LOCKER
        // =====================================================

        Locker locker =
            hit.collider.GetComponentInParent<Locker>();

        if (locker != null)
        {
            if (lockerEImage != null)
                lockerEImage.SetActive(false);

            HideAllPickupEImages();

            locker.OpenKeypad();

            return;
        }


        // =====================================================
        // PICKUP OBJECT
        // =====================================================

        if (!hit.collider.CompareTag("PickupObject"))
            return;

        heldItem = hit.collider.gameObject;


        // Save original world scale BEFORE parenting
        originalWorldScale =
            heldItem.transform.lossyScale;


        // Disable physics
        Rigidbody rb =
            heldItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }


        // Disable collider
        Collider col =
            heldItem.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;


        // Move into hand
        heldItem.transform.SetParent(
            objectHoldPoint,
            false
        );

        heldItem.transform.localPosition =
            Vector3.zero;

        heldItem.transform.localRotation =
            Quaternion.identity;


        // Keep original world size
        Vector3 parentScale =
            objectHoldPoint.lossyScale;

        heldItem.transform.localScale =
            new Vector3(
                originalWorldScale.x / parentScale.x,
                originalWorldScale.y / parentScale.y,
                originalWorldScale.z / parentScale.z
            );


        // Hide body while holding
        if (ch01Body != null)
            ch01Body.SetActive(false);


        HideAllPickupEImages();

        Debug.Log(
            "Picked up: " +
            heldItem.name
        );
    }


    // =========================================================
    // DROP
    // =========================================================

    void Drop()
    {
        if (heldItem == null)
            return;


        // Remove from hand
        heldItem.transform.SetParent(null);


        // Drop in front of player
        heldItem.transform.position =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            dropDistance;


        // Enable collider
        Collider col =
            heldItem.GetComponent<Collider>();

        if (col != null)
            col.enabled = true;


        // Enable physics
        Rigidbody rb =
            heldItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }


        // Enable body
        if (ch01Body != null)
            ch01Body.SetActive(true);


        heldItem = null;

        Debug.Log(
            "Dropped item."
        );
    }
}