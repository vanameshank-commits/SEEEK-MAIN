using UnityEngine;

public class FenceWarning : MonoBehaviour
{
    public GameObject warningText;

    void Start()
    {
        if (warningText != null)
            warningText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER ENTERED FENCE WARNING");

            if (warningText != null)
                warningText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (warningText != null)
                warningText.SetActive(false);
        }
    }
}