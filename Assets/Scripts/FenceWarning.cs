using UnityEngine;
using TMPro;

public class FenceWarning : MonoBehaviour
{
    public GameObject warningText;

    private void Start()
    {
        if (warningText != null)
            warningText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (warningText != null)
                warningText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (warningText != null)
                warningText.SetActive(false);
        }
    }
}