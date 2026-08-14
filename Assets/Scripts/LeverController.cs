using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("References")]
    public Transform leverTransform;
    public FenceController fence;
    public Camera playerCamera;
    public GameObject eImage;

    [Header("Settings")]
    public float interactDistance = 3f;
    public float downAngle = 45f;
    public float returnSpeed = 3f;

    private Quaternion startRotation;
    private bool canUse = true;

    void Start()
    {
        if (leverTransform == null)
            leverTransform = transform;

        startRotation = leverTransform.localRotation;

        if (eImage != null)
            eImage.SetActive(false);
    }

    void Update()
    {
        UpdateEImage();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryUseLever();
        }
    }

    void UpdateEImage()
    {
        if (eImage == null || playerCamera == null)
            return;

        eImage.SetActive(false);

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            LeverController lever =
                hit.collider.GetComponentInParent<LeverController>();

            if (lever == this && canUse)
            {
                eImage.SetActive(true);
            }
        }
    }

    void TryUseLever()
    {
        if (!canUse)
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
            LeverController lever =
                hit.collider.GetComponentInParent<LeverController>();

            if (lever == this)
            {
                UseLever();
            }
        }
    }

    public void UseLever()
    {
        if (!canUse)
            return;

        canUse = false;

        if (eImage != null)
            eImage.SetActive(false);

        StartCoroutine(PullLever());
    }

    System.Collections.IEnumerator PullLever()
    {
          Quaternion downRotation =
           startRotation *
           Quaternion.AngleAxis(
           downAngle,
           Vector3.right

    );

        float t = 0f;

        // Lever goes down
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;

            leverTransform.localRotation =
                Quaternion.Slerp(
                    startRotation,
                    downRotation,
                    t
                );

            yield return null;
        }

        leverTransform.localRotation =
            downRotation;

        // Tell fence to move
        if (fence != null)
        {
            fence.DropFence();
        }

        yield return new WaitForSeconds(0.5f);

        // Lever comes back up
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;

            leverTransform.localRotation =
                Quaternion.Slerp(
                    downRotation,
                    startRotation,
                    t
                );

            yield return null;
        }

        leverTransform.localRotation =
            startRotation;

        canUse = true;
    }
}