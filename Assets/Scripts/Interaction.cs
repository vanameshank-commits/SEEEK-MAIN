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

    // =========================================================
    // INVENTORY
    // =========================================================

    [Header("Inventory Images")]
    public GameObject zItemImage;
    public GameObject xItemImage;
    public GameObject cItemImage;
    public GameObject vItemImage;

    [Header("Inventory Objects")]
    public GameObject cube;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;

    private bool cubeCollected = false;
    private bool cube1Collected = false;
    private bool cube2Collected = false;
    private bool cube3Collected = false;

    private GameObject heldItem;

    private Vector3 originalWorldScale;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        if (zItemImage != null)
            zItemImage.SetActive(false);

        if (xItemImage != null)
            xItemImage.SetActive(false);

        if (cItemImage != null)
            cItemImage.SetActive(false);

        if (vItemImage != null)
            vItemImage.SetActive(false);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        SyncHeldItem();

        UpdatePickupUI();
        UpdateLockerUI();

        // E = Pickup
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }

        // Z = Cube
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleInventoryItem(0);
        }

        // X = Cube 1
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleInventoryItem(1);
        }

        // C = Cube 2
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleInventoryItem(2);
        }

        // V = Cube 3
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleInventoryItem(3);
        }

        // Q = Drop
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

        string objectName = hit.collider.gameObject.name;


        // CUBE
        if (objectName == "Cube")
        {
            if (pickupEImage1 != null)
                pickupEImage1.SetActive(true);
        }

        // CUBE 1
        else if (objectName == "Cube (1)" ||
                 objectName == "Cube 1")
        {
            if (pickupEImage2 != null)
                pickupEImage2.SetActive(true);
        }

        // CUBE 2
        else if (objectName == "Cube (2)" ||
                 objectName == "Cube 2")
        {
            if (pickupEImage3 != null)
                pickupEImage3.SetActive(true);
        }

        // CUBE 3
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

        GameObject pickedObject =
            hit.collider.gameObject;


        // =====================================================
        // CUBE
        // =====================================================

        if (pickedObject == cube)
        {
            if (cubeCollected)
                return;

            cubeCollected = true;

            if (zItemImage != null)
                zItemImage.SetActive(true);

            pickedObject.SetActive(false);

            Debug.Log("Cube added to Z inventory.");

            HideAllPickupEImages();

            return;
        }


        // =====================================================
        // CUBE 1
        // =====================================================

        if (pickedObject == cube1)
        {
            if (cube1Collected)
                return;

            cube1Collected = true;

            if (xItemImage != null)
                xItemImage.SetActive(true);

            pickedObject.SetActive(false);

            Debug.Log("Cube 1 added to X inventory.");

            HideAllPickupEImages();

            return;
        }


        // =====================================================
        // CUBE 2
        // =====================================================

        if (pickedObject == cube2)
        {
            if (cube2Collected)
                return;

            cube2Collected = true;

            if (cItemImage != null)
                cItemImage.SetActive(true);

            pickedObject.SetActive(false);

            Debug.Log("Cube 2 added to C inventory.");

            HideAllPickupEImages();

            return;
        }


        // =====================================================
        // CUBE 3
        // =====================================================

        if (pickedObject == cube3)
        {
            if (cube3Collected)
                return;

            cube3Collected = true;

            if (vItemImage != null)
                vItemImage.SetActive(true);

            pickedObject.SetActive(false);

            Debug.Log("Cube 3 added to V inventory.");

            HideAllPickupEImages();

            return;
        }
    }


    // =========================================================
    // INVENTORY TOGGLE
    // =========================================================

    void ToggleInventoryItem(int index)
    {
        GameObject item = GetInventoryItem(index);

        if (item == null)
            return;

        if (!IsCollected(index))
            return;


        // =====================================================
        // SAME ITEM ALREADY IN HAND
        // =====================================================

        if (heldItem == item)
        {
            ReturnToInventory(index);
            return;
        }


        // =====================================================
        // ANOTHER ITEM IS IN HAND
        // =====================================================

        if (heldItem != null)
        {
            Debug.Log("Another item is already in hand.");
            return;
        }


        // =====================================================
        // TAKE ITEM FROM INVENTORY
        // =====================================================

        EquipItem(item);
    }


    // =========================================================
    // EQUIP ITEM
    // =========================================================

    void EquipItem(GameObject item)
    {
        heldItem = item;


        // =====================================================
        // HIDE INVENTORY IMAGE
        // =====================================================

        if (item == cube)
        {
            if (zItemImage != null)
                zItemImage.SetActive(false);
        }
        else if (item == cube1)
        {
            if (xItemImage != null)
                xItemImage.SetActive(false);
        }
        else if (item == cube2)
        {
            if (cItemImage != null)
                cItemImage.SetActive(false);
        }
        else if (item == cube3)
        {
            if (vItemImage != null)
                vItemImage.SetActive(false);
        }


        // =====================================================
        // ACTIVATE OBJECT
        // =====================================================

        heldItem.SetActive(true);


        // Save scale
        originalWorldScale =
            heldItem.transform.lossyScale;


        // =====================================================
        // DISABLE PHYSICS
        // =====================================================

        Rigidbody rb =
            heldItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }


        // =====================================================
        // DISABLE COLLIDER
        // =====================================================

        Collider col =
            heldItem.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;


        // =====================================================
        // MOVE TO EXISTING HAND POINT
        // =====================================================

        heldItem.transform.SetParent(
            objectHoldPoint,
            false
        );

        heldItem.transform.localPosition =
            Vector3.zero;

        heldItem.transform.localRotation =
            Quaternion.identity;


        // =====================================================
        // KEEP ORIGINAL WORLD SIZE
        // =====================================================

        Vector3 parentScale =
            objectHoldPoint.lossyScale;

        heldItem.transform.localScale =
            new Vector3(
                originalWorldScale.x / parentScale.x,
                originalWorldScale.y / parentScale.y,
                originalWorldScale.z / parentScale.z
            );


        // =====================================================
        // HIDE FIRST PERSON BODY
        // =====================================================

        if (ch01Body != null)
            ch01Body.SetActive(false);


        Debug.Log(
            "Equipped: " +
            heldItem.name +
            " | Inventory image hidden."
        );
    }


    // =========================================================
    // RETURN ITEM TO INVENTORY
    // =========================================================

    void ReturnToInventory(int index)
    {
        if (heldItem == null)
            return;

        GameObject item = heldItem;


        // =====================================================
        // REMOVE FROM HAND
        // =====================================================

        item.transform.SetParent(null);


        // =====================================================
        // HIDE OBJECT
        // =====================================================

        item.SetActive(false);


        // =====================================================
        // KEEP PHYSICS DISABLED
        // =====================================================

        Rigidbody rb =
            item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }


        // =====================================================
        // KEEP COLLIDER DISABLED
        // =====================================================

        Collider col =
            item.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;


        heldItem = null;


        // =====================================================
        // SHOW INVENTORY IMAGE AGAIN
        // =====================================================

        if (item == cube)
        {
            if (zItemImage != null)
                zItemImage.SetActive(true);
        }
        else if (item == cube1)
        {
            if (xItemImage != null)
                xItemImage.SetActive(true);
        }
        else if (item == cube2)
        {
            if (cItemImage != null)
                cItemImage.SetActive(true);
        }
        else if (item == cube3)
        {
            if (vItemImage != null)
                vItemImage.SetActive(true);
        }


        // =====================================================
        // SHOW FIRST PERSON BODY
        // =====================================================

        if (ch01Body != null)
            ch01Body.SetActive(true);


        Debug.Log(
            "Returned to inventory: " +
            item.name +
            " | Inventory image visible."
        );
    }


    // =========================================================
    // GET INVENTORY ITEM
    // =========================================================

    GameObject GetInventoryItem(int index)
    {
        if (index == 0)
            return cube;

        if (index == 1)
            return cube1;

        if (index == 2)
            return cube2;

        if (index == 3)
            return cube3;

        return null;
    }


    // =========================================================
    // CHECK COLLECTED
    // =========================================================

    bool IsCollected(int index)
    {
        if (index == 0)
            return cubeCollected;

        if (index == 1)
            return cube1Collected;

        if (index == 2)
            return cube2Collected;

        if (index == 3)
            return cube3Collected;

        return false;
    }


    // =========================================================
    // DROP
    // =========================================================

    void Drop()
    {
        if (heldItem == null)
            return;

        GameObject droppedItem =
            heldItem;


        // =====================================================
        // REMOVE FROM HAND
        // =====================================================

        droppedItem.transform.SetParent(null);

        droppedItem.transform.position =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            dropDistance;


        // =====================================================
        // ENABLE COLLIDER
        // =====================================================

        Collider col =
            droppedItem.GetComponent<Collider>();

        if (col != null)
            col.enabled = true;


        // =====================================================
        // ENABLE PHYSICS
        // =====================================================

        Rigidbody rb =
            droppedItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }


        // =====================================================
        // REMOVE FROM INVENTORY
        // =====================================================

        RemoveFromInventory(droppedItem);


        // =====================================================
        // SHOW BODY
        // =====================================================

        if (ch01Body != null)
            ch01Body.SetActive(true);


        heldItem = null;

        Debug.Log("Dropped item.");
    }


    // =========================================================
    // REMOVE FROM INVENTORY
    // =========================================================

    void RemoveFromInventory(GameObject item)
    {
        if (item == cube)
        {
            cubeCollected = false;

            if (zItemImage != null)
                zItemImage.SetActive(false);
        }

        else if (item == cube1)
        {
            cube1Collected = false;

            if (xItemImage != null)
                xItemImage.SetActive(false);
        }

        else if (item == cube2)
        {
            cube2Collected = false;

            if (cItemImage != null)
                cItemImage.SetActive(false);
        }

        else if (item == cube3)
        {
            cube3Collected = false;

            if (vItemImage != null)
                vItemImage.SetActive(false);
        }
    }
}