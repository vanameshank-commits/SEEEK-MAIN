using UnityEngine;
using System.Collections;

public class DeskCubePlacement : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform objectHoldPoint;

    [Header("Cube Place Points")]
    public Transform cubePlacePoint1;
    public Transform cubePlacePoint2;

    [Header("Place Text")]
    public GameObject placeText1;
    public GameObject placeText2;

    [Header("Rotate Text")]
    public GameObject rotateText1;
    public GameObject rotateText2;

    [Header("Normal Interaction")]
    public Interaction interaction;

    [Header("Settings")]
    public float interactDistance = 3f;

    private GameObject placedCube1;
    private GameObject placedCube2;

    private bool rotationMode = false;
    private int rotatingSlot = -1;


    void Start()
    {
        HideAllTexts();
    }


    void Update()
    {
        // =====================================================
        // ROTATION MODE
        // =====================================================

        if (rotationMode)
        {
            UpdateRotationUI();

            // E = place cube back
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlaceCubeBack();
            }

            return;
        }


        // =====================================================
        // CHECK FOR ROTATION OF PLACED CUBES
        // =====================================================

        UpdateRotateUI();

        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) ||
            Input.GetKeyDown(KeyCode.Alpha4))
        {
            TryStartRotation();
        }


        // =====================================================
        // CHECK FOR PLACE
        // =====================================================

        UpdatePlaceUI();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlaceCube();
        }
    }


    // =========================================================
    // HIDE ALL TEXTS
    // =========================================================

    void HideAllTexts()
    {
        if (placeText1 != null)
            placeText1.SetActive(false);

        if (placeText2 != null)
            placeText2.SetActive(false);

        if (rotateText1 != null)
            rotateText1.SetActive(false);

        if (rotateText2 != null)
            rotateText2.SetActive(false);
    }


    // =========================================================
    // GET HELD CUBE
    // =========================================================

    GameObject GetHeldCube()
    {
        if (objectHoldPoint == null)
            return null;

        for (int i = 0; i < objectHoldPoint.childCount; i++)
        {
            Transform child =
                objectHoldPoint.GetChild(i);

            if (child.CompareTag("PickupObject"))
            {
                return child.gameObject;
            }
        }

        return null;
    }


    // =========================================================
    // FIND FREE SLOT
    // =========================================================

    int GetFreeSlot()
    {
        if (placedCube1 == null)
            return 1;

        if (placedCube2 == null)
            return 2;

        return -1;
    }


    // =========================================================
    // PLACE UI
    // =========================================================

    void UpdatePlaceUI()
    {
        if (playerCamera == null)
            return;

        if (placedCube1 != null &&
            placedCube2 != null)
        {
            return;
        }

        GameObject heldCube =
            GetHeldCube();

        if (heldCube == null)
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

        DeskCubePlacement desk =
            hit.collider.GetComponentInParent<DeskCubePlacement>();

        if (desk != this)
            return;

        int freeSlot =
            GetFreeSlot();

        if (freeSlot == 1)
        {
            if (placeText1 != null)
                placeText1.SetActive(true);
        }
        else if (freeSlot == 2)
        {
            if (placeText2 != null)
                placeText2.SetActive(true);
        }
    }


    // =========================================================
    // PLACE CUBE
    // =========================================================

    void TryPlaceCube()
    {
        GameObject cube =
            GetHeldCube();

        if (cube == null)
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

        DeskCubePlacement desk =
            hit.collider.GetComponentInParent<DeskCubePlacement>();

        if (desk != this)
            return;

        int freeSlot =
            GetFreeSlot();

        if (freeSlot == -1)
            return;

        PlaceCube(cube, freeSlot);
    }


    // =========================================================
    // PLACE CUBE ON DESK
    // =========================================================

    void PlaceCube(
        GameObject cube,
        int slot)
    {
        cube.tag = "PlacedCube";

        cube.transform.SetParent(null);

        Transform targetPoint = null;

        if (slot == 1)
        {
            placedCube1 = cube;
            targetPoint = cubePlacePoint1;

            if (placeText1 != null)
                placeText1.SetActive(false);

            if (rotateText1 != null)
                rotateText1.SetActive(true);
        }
        else
        {
            placedCube2 = cube;
            targetPoint = cubePlacePoint2;

            if (placeText2 != null)
                placeText2.SetActive(false);

            if (rotateText2 != null)
                rotateText2.SetActive(true);
        }

        if (targetPoint != null)
        {
            cube.transform.position =
                targetPoint.position;

            cube.transform.rotation =
                targetPoint.rotation;
        }

        Collider col =
            cube.GetComponent<Collider>();

        if (col != null)
            col.enabled = true;

        Rigidbody rb =
            cube.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Debug.Log(
            "Cube placed in slot " +
            slot
        );
    }


    // =========================================================
    // ROTATE UI
    // =========================================================

    void UpdateRotateUI()
    {
        if (playerCamera == null)
            return;

        if (rotateText1 != null)
            rotateText1.SetActive(false);

        if (rotateText2 != null)
            rotateText2.SetActive(false);

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

        GameObject hitObject =
            hit.collider.gameObject;

        if (hitObject.CompareTag("PlacedCube"))
        {
            if (hitObject == placedCube1)
            {
                if (rotateText1 != null)
                    rotateText1.SetActive(true);
            }
            else if (hitObject == placedCube2)
            {
                if (rotateText2 != null)
                    rotateText2.SetActive(true);
            }
        }
    }


    // =========================================================
    // START ROTATION
    // =========================================================

    void TryStartRotation()
    {
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

        GameObject hitObject =
            hit.collider.gameObject;

        int slot = -1;

        if (hitObject == placedCube1)
        {
            slot = 1;
        }
        else if (hitObject == placedCube2)
        {
            slot = 2;
        }

        if (slot == -1)
            return;

        GameObject cube =
            slot == 1
            ? placedCube1
            : placedCube2;

        CubeRotation rotation =
            cube.GetComponent<CubeRotation>();

        if (rotation == null)
        {
            Debug.LogError(
                "CubeRotation missing on cube!"
            );

            return;
        }

        rotationMode = true;
        rotatingSlot = slot;

        // Disable normal interaction
        if (interaction != null)
            interaction.enabled = false;

        if (rotateText1 != null)
            rotateText1.SetActive(false);

        if (rotateText2 != null)
            rotateText2.SetActive(false);

        rotation.StartRotation();

        Debug.Log(
            "Rotation started for slot " +
            slot
        );
    }


    // =========================================================
    // ROTATION UI
    // =========================================================

    void UpdateRotationUI()
    {
        if (placeText1 != null)
            placeText1.SetActive(true);

        if (placeText2 != null)
            placeText2.SetActive(false);

        if (rotateText1 != null)
            rotateText1.SetActive(false);

        if (rotateText2 != null)
            rotateText2.SetActive(false);

        if (rotatingSlot == 2)
        {
            if (placeText1 != null)
                placeText1.SetActive(false);

            if (placeText2 != null)
                placeText2.SetActive(true);
        }
    }


    // =========================================================
    // PLACE BACK
    // =========================================================

    void PlaceCubeBack()
    {
        GameObject cube =
            rotatingSlot == 1
            ? placedCube1
            : placedCube2;

        if (cube == null)
            return;

        CubeRotation rotation =
            cube.GetComponent<CubeRotation>();

        if (rotation != null)
            rotation.StopRotation();

        cube.transform.SetParent(null);

        Transform targetPoint =
            rotatingSlot == 1
            ? cubePlacePoint1
            : cubePlacePoint2;

        if (targetPoint != null)
        {
            cube.transform.position =
                targetPoint.position;
        }

        Rigidbody rb =
            cube.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col =
            cube.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;

        rotationMode = false;

        if (rotatingSlot == 1)
        {
            if (rotateText1 != null)
                rotateText1.SetActive(true);
        }
        else
        {
            if (rotateText2 != null)
                rotateText2.SetActive(true);
        }

        StartCoroutine(RestoreInteractionAfterE());

        Debug.Log(
            "Cube placed back in slot " +
            rotatingSlot
        );
    }


    // =========================================================
    // RESTORE INTERACTION
    // =========================================================

    IEnumerator RestoreInteractionAfterE()
    {
        while (Input.GetKey(KeyCode.E))
        {
            yield return null;
        }

        yield return null;

        GameObject cube =
            rotatingSlot == 1
            ? placedCube1
            : placedCube2;

        if (cube != null)
        {
            Collider col =
                cube.GetComponent<Collider>();

            if (col != null)
                col.enabled = true;
        }

        if (interaction != null)
            interaction.enabled = true;

        rotatingSlot = -1;

        Debug.Log(
            "Normal interaction restored."
        );
    }
}