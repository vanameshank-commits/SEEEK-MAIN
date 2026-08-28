using UnityEngine;

public class NoteBook : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Canvas EImage;

    [Header("This Book's Note")]
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
            // Check whether the ray actually hit THIS notebook
            NoteBook hitBook = hit.collider.GetComponentInParent<NoteBook>();

            if (hitBook == this)
            {
                lookingAtBook = true;
            }
        }

        // SHOW / HIDE E IMAGE
        EImage.gameObject.SetActive(lookingAtBook);

        // OPEN ONLY THIS BOOK'S NOTE
        if (lookingAtBook && Input.GetKeyDown(KeyCode.E))
        {
            EImage.gameObject.SetActive(false);
            NoteImage.SetActive(true);
            noteOpen = true;
        }
    }
}