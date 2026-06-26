using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flash;

    public AudioSource audioSource;

    void Start()
    {
        if (flash != null)
            flash.enabled = false;
    }

    void Update()
    {
        if (
        Input.GetKeyDown(
        KeyCode.F
        ))
        {
            flash.enabled =
            !flash.enabled;

            if (
            audioSource != null
            &&
            audioSource.clip != null
            )
            {
                audioSource.Play();
            }
        }
    }
}