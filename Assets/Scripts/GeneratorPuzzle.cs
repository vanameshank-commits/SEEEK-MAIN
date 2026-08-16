using UnityEngine;
using UnityEngine.UI;

public class GeneratorPuzzle : MonoBehaviour
{
    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]
    public GameObject generatorPanel;
    public Slider progressSlider;

    // Image that says "E"
    public GameObject interactImage;


    // =========================================================
    // BUTTONS
    // =========================================================

    [Header("Buttons")]
    public Button[] buttons;


    // =========================================================
    // GENERATOR
    // =========================================================

    [Header("Generator")]
    public GameObject generatorObject;


    // =========================================================
    // REWARD
    // =========================================================

    [Header("Reward")]
    public GameObject cube3;


    // =========================================================
    // LIGHTS
    // =========================================================

    [Header("Lights After Generator")]
    public GameObject[] lightsToTurnOn;


    // =========================================================
    // MUSIC
    // =========================================================

    [Header("Music After Generator")]
    public AudioSource music;


    // =========================================================
    // INTERACTION
    // =========================================================

    [Header("Interaction")]
    public Camera playerCamera;
    public float interactDistance = 3f;


    // =========================================================
    // VARIABLES
    // =========================================================

    private bool panelOpen = false;
    private bool solved = false;

    private int currentCorrectButton = 0;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        // Hide generator panel
        if (generatorPanel != null)
            generatorPanel.SetActive(false);

        // Hide E image
        if (interactImage != null)
            interactImage.SetActive(false);

        // Hide Cube 3
        if (cube3 != null)
            cube3.SetActive(false);

        // Turn lights OFF initially
        if (lightsToTurnOn != null)
        {
            foreach (GameObject lightObject in lightsToTurnOn)
            {
                if (lightObject != null)
                    lightObject.SetActive(false);
            }
        }

        // Music OFF initially
        if (music != null)
        {
            music.Stop();
        }

        // Slider
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = buttons.Length;
            progressSlider.value = 0;
        }

        // Button setup
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i;

            if (buttons[i] != null)
            {
                buttons[i].onClick.AddListener(
                    () => PressButton(buttonIndex)
                );
            }
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (solved)
            return;

        // If panel is open
        if (panelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ClosePanel();
            }

            return;
        }

        // Check if player is looking at generator
        CheckPlayerLookingAtGenerator();
    }


    // =========================================================
    // CHECK PLAYER LOOKING
    // =========================================================

    void CheckPlayerLookingAtGenerator()
    {
        if (playerCamera == null)
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
            GeneratorPuzzle generator =
                hit.collider.GetComponentInParent<GeneratorPuzzle>();

            if (generator == this)
            {
                // Show E image
                if (interactImage != null)
                    interactImage.SetActive(true);

                // Press E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenPanel();
                }

                return;
            }
        }

        // Player is NOT looking at generator
        if (interactImage != null)
            interactImage.SetActive(false);
    }


    // =========================================================
    // OPEN PANEL
    // =========================================================

    void OpenPanel()
    {
        if (solved)
            return;

        panelOpen = true;

        // Hide E image
        if (interactImage != null)
            interactImage.SetActive(false);

        // Show generator panel
        if (generatorPanel != null)
            generatorPanel.SetActive(true);

        // Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Generator panel opened.");
    }


    // =========================================================
    // CLOSE PANEL
    // =========================================================

    void ClosePanel()
    {
        panelOpen = false;

        if (generatorPanel != null)
            generatorPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Generator panel closed.");
    }


    // =========================================================
    // BUTTON
    // =========================================================

    void PressButton(int buttonIndex)
    {
        if (!panelOpen)
            return;

        if (solved)
            return;


        // Correct button
        if (buttonIndex == currentCorrectButton)
        {
            currentCorrectButton++;

            if (progressSlider != null)
            {
                progressSlider.value =
                    currentCorrectButton;
            }

            Debug.Log(
                "Correct generator button: " +
                (buttonIndex + 1)
            );


            // Completed
            if (currentCorrectButton >= buttons.Length)
            {
                CompletePuzzle();
            }
        }
        else
        {
            Debug.Log("Wrong generator button!");
        }
    }


    // =========================================================
    // COMPLETE GENERATOR
    // =========================================================

    void CompletePuzzle()
    {
        solved = true;
        panelOpen = false;


        // -----------------------------------------------------
        // CLOSE PANEL
        // -----------------------------------------------------

        if (generatorPanel != null)
            generatorPanel.SetActive(false);


        // -----------------------------------------------------
        // HIDE GENERATOR
        // -----------------------------------------------------

        if (generatorObject != null)
        {
            generatorObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // SHOW CUBE 3
        // -----------------------------------------------------

        if (cube3 != null)
        {
            cube3.SetActive(true);
        }


        // -----------------------------------------------------
        // TURN LIGHTS ON
        // -----------------------------------------------------

        if (lightsToTurnOn != null)
        {
            foreach (GameObject lightObject in lightsToTurnOn)
            {
                if (lightObject != null)
                {
                    lightObject.SetActive(true);
                }
            }
        }


        // -----------------------------------------------------
        // START MUSIC
        // -----------------------------------------------------

        if (music != null)
        {
            music.Play();
        }


        // -----------------------------------------------------
        // LOCK CURSOR
        // -----------------------------------------------------

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        Debug.Log(
            "================================="
        );

        Debug.Log(
            "GENERATOR PUZZLE COMPLETE!"
        );

        Debug.Log(
            "GENERATOR DISAPPEARED"
        );

        Debug.Log(
            "LIGHTS TURNED ON"
        );

        Debug.Log(
            "MUSIC STARTED"
        );

        Debug.Log(
            "CUBE 3 IS NOW VISIBLE"
        );

        Debug.Log(
            "================================="
        );
    }
}