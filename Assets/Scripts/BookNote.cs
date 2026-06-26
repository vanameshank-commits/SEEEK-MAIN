using UnityEngine;

public class NoteBook : MonoBehaviour
{
    public Canvas EImage;


public GameObject NoteImage;

    bool near;

    void Start()
    {
        EImage.gameObject.SetActive(
        false);

        NoteImage.SetActive(
        false);
    }

    void Update()
    {
        if (
        near
        )
        {
            EImage.gameObject.SetActive(
            true);

            if (
            Input.GetKeyDown(
            KeyCode.E))
            {
                EImage.gameObject.SetActive(
                false);

                NoteImage.SetActive(
                true);
            }
        }

        if (
        !near
        )
        {
            EImage.gameObject.SetActive(
            false);
        }

        if (
        NoteImage.activeSelf
        &&
        Input.GetKeyDown(
        KeyCode.Q))
        {
            NoteImage.SetActive(
            false);
        }
    }

    private void OnTriggerEnter(
    Collider other)
    {
        if (
        other.CompareTag(
        "Player"))
        {
            near = true;
        }
    }

    private void OnTriggerExit(
    Collider other)
    {
        if (
        other.CompareTag(
        "Player"))
        {
            near = false;

            EImage.gameObject.SetActive(
            false);
        }
    }


}
