using UnityEngine;

public class NoteBook : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Canvas EImage;
    public GameObject NoteImage;

    [Header("Settings")]
    public float interactDistance = 3f;

    private bool noteOpen = false;

    void Start()
    {
        EImage.gameObject.SetActive(false);
        NoteImage.SetActive(false);
    }

    void Update()
    {
        // NOTE IS OPEN
        if (noteOpen)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                NoteImage.SetActive(false);
                noteOpen = false;
            }

            return;
        }

        // CHECK BOOK
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        bool lookingAtBook = false;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            if (hit.collider.CompareTag("Book"))
            {
                lookingAtBook = true;
            }
        }

        // SHOW / HIDE E IMAGE
        EImage.gameObject.SetActive(lookingAtBook);

        // OPEN NOTE
        if (lookingAtBook && Input.GetKeyDown(KeyCode.E))
        {
            EImage.gameObject.SetActive(false);
            NoteImage.SetActive(true);
            noteOpen = true;
        }
    }
}