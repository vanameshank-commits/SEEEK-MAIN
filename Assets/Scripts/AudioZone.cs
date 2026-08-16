using UnityEngine;

public class AudioZone : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        Debug.Log("Player entered audio zone.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log("Player left audio zone.");
    }
}