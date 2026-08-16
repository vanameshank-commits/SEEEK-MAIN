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

    private Vector3 originalWorldScale;


    void Update()
    {
        SyncHeldItem();

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
    // SYNC HELD ITEM
    // =========================================================

    void SyncHeldItem()
    {
        if (heldItem == null)
            return;

        if (heldItem.transform.parent != objectHoldPoint)
        {
            heldItem = null;

            if (ch01Body != null)
                ch01Body.SetActive(true);

            Debug.Log("Held item released.");
        }
    }


    // =========================================================
    // HIDE ALL PICKUP E IMAGES
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

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            return;
        }

        if (!hit.collider.CompareTag("PickupObject"))
            return;

        string objectName =
            hit.collider.gameObject.name;


        // =====================================================
        // CUBE 1
        // =====================================================

        if (objectName == "Cube")
        {
            if (pickupEImage1 != null)
                pickupEImage1.SetActive(true);
        }


        // =====================================================
        // CUBE 2
        // =====================================================

        else if (objectName == "Cube (1)" ||
                 objectName == "Cube 1")
        {
            if (pickupEImage2 != null)
                pickupEImage2.SetActive(true);
        }


        // =====================================================
        // CUBE 3
        // =====================================================

        else if (objectName == "Cube (2)" ||
                 objectName == "Cube 2")
        {
            if (pickupEImage3 != null)
                pickupEImage3.SetActive(true);
        }


        // =====================================================
        // CUBE 4
        // =====================================================

        else if (objectName == "Cube (3)" ||
                 objectName == "Cube 3")
        {
            if (pickupEImage4 != null)
                pickupEImage4.SetActive(true);
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
    // PICKUP
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


        // Save original world scale
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


        // Hide body
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

        heldItem.transform.SetParent(null);

        heldItem.transform.position =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            dropDistance;


        Collider col =
            heldItem.GetComponent<Collider>();

        if (col != null)
            col.enabled = true;


        Rigidbody rb =
            heldItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }


        if (ch01Body != null)
            ch01Body.SetActive(true);

        heldItem = null;

        Debug.Log("Dropped item.");
    }
}