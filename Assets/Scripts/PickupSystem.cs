using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    private GameObject objectToPick;

    public Transform holdPoint;

    private Canvas EImage;

    bool picked;
    bool near;

    Rigidbody rb;

    void Update()
    {
        if (near && !picked)
        {
            if (EImage != null)
                EImage.gameObject.SetActive(true);
        }
        else
        {
            if (EImage != null)
                EImage.gameObject.SetActive(false);
        }

        if (
            near
            &&
            objectToPick != null
            &&
            Input.GetKeyDown(KeyCode.E)
            &&
            !picked
        )
        {
            picked = true;

            if (EImage != null)
                EImage.gameObject.SetActive(false);

            objectToPick.transform.position =
                holdPoint.position;

            objectToPick.transform.SetParent(
                holdPoint
            );

            if (rb != null)
                rb.isKinematic = true;
        }

        if (picked)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                picked = false;

                if (objectToPick != null)
                {
                    objectToPick.transform.parent =
                        null;
                }

                if (rb != null)
                    rb.isKinematic = false;

                if (EImage != null)
                    EImage.gameObject.SetActive(false);

                objectToPick = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (picked)
            return;

        if (other.CompareTag("Object"))
        {
            near = true;

            objectToPick =
                other.gameObject;

            rb =
                other.GetComponent<Rigidbody>();

            if (
                other.transform.childCount > 0
            )
            {
                EImage =
                    other.transform
                    .GetChild(0)
                    .GetComponent<Canvas>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (picked)
            return;

        if (other.CompareTag("Object"))
        {
            near = false;

            if (EImage != null)
                EImage.gameObject.SetActive(false);

            objectToPick = null;

            rb = null;

            EImage = null;
        }
    }
}