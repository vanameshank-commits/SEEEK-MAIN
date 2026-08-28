using UnityEngine;
using UnityEngine.UI;

public class GeneratorPuzzle : MonoBehaviour
{
    [Header("UI")]
    public GameObject generatorPanel;
    public Slider progressSlider;
    public GameObject interactImage;

    [Header("Buttons")]
    public Button[] buttons;

    [Header("Correct Sequence")]
    public int[] correctSequence = { 1, 2, 3, 4, 5 };

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    [Header("Generator")]
    public GameObject generatorObject;

    [Header("Reward")]
    public GameObject cube3;

    [Header("Lights After Generator")]
    public GameObject[] lightsToTurnOn;

    [Header("Music After Generator")]
    public AudioSource music;

    [Header("Interaction")]
    public Camera playerCamera;
    public float interactDistance = 3f;

    [Header("Camera Look")]
    public MonoBehaviour cameraLook;

    private bool panelOpen = false;
    private bool solved = false;

    private int currentStep = 0;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        if (generatorPanel != null)
            generatorPanel.SetActive(false);

        if (interactImage != null)
            interactImage.SetActive(false);

        if (cube3 != null)
            cube3.SetActive(false);

        if (lightsToTurnOn != null)
        {
            foreach (GameObject lightObject in lightsToTurnOn)
            {
                if (lightObject != null)
                    lightObject.SetActive(false);
            }
        }

        if (music != null)
            music.Stop();


        // SLIDER
        if (progressSlider != null)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = correctSequence.Length;
            progressSlider.value = 0;
        }


        // BUTTONS
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonNumber = i + 1;

            if (buttons[i] != null)
            {
                buttons[i].onClick.RemoveAllListeners();

                buttons[i].onClick.AddListener(
                    () => PressButton(buttonNumber)
                );

                SetButtonColor(buttons[i], normalColor);
            }
        }

        Debug.Log("Generator buttons initialized: " + buttons.Length);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (solved)
            return;

        if (panelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ClosePanel();
            }

            return;
        }

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
                if (interactImage != null)
                    interactImage.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenPanel();
                }

                return;
            }
        }

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

        if (interactImage != null)
            interactImage.SetActive(false);

        if (generatorPanel != null)
            generatorPanel.SetActive(true);


        // =====================================================
        // STOP ONLY CAMERA LOOK
        // =====================================================

        if (cameraLook != null)
        {
            cameraLook.enabled = false;
        }


        // Unlock mouse for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Generator panel OPENED - CAMERA LOOK LOCKED");
    }


    // =========================================================
    // CLOSE PANEL
    // =========================================================

    void ClosePanel()
    {
        panelOpen = false;

        if (generatorPanel != null)
            generatorPanel.SetActive(false);


        // =====================================================
        // ENABLE CAMERA LOOK AGAIN
        // =====================================================

        if (cameraLook != null)
        {
            cameraLook.enabled = true;
        }


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Generator panel CLOSED - CAMERA LOOK ENABLED");
    }


    // =========================================================
    // BUTTON PRESSED
    // =========================================================

    public void PressButton(int buttonNumber)
    {
        if (solved)
            return;

        if (currentStep >= correctSequence.Length)
            return;

        Debug.Log("BUTTON PRESSED: " + buttonNumber);

        int correctButton = correctSequence[currentStep];

        if (buttonNumber == correctButton)
        {
            Debug.Log("CORRECT!");

            SetButtonColor(
                buttons[buttonNumber - 1],
                correctColor
            );

            currentStep++;

            if (progressSlider != null)
                progressSlider.value = currentStep;

            if (currentStep >= correctSequence.Length)
            {
                SolvePuzzle();
            }
        }
        else
        {
            Debug.Log("WRONG BUTTON!");

            SetButtonColor(
                buttons[buttonNumber - 1],
                wrongColor
            );

            CancelInvoke(nameof(ResetPuzzle));
            Invoke(nameof(ResetPuzzle), 0.5f);
        }
    }


    // =========================================================
    // RESET
    // =========================================================

    void ResetPuzzle()
    {
        currentStep = 0;

        if (progressSlider != null)
            progressSlider.value = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
                SetButtonColor(buttons[i], normalColor);
        }

        Debug.Log("Sequence reset.");
    }


    // =========================================================
    // SOLVE
    // =========================================================

    void SolvePuzzle()
    {
        solved = true;

        Debug.Log("GENERATOR PUZZLE SOLVED!");

        if (lightsToTurnOn != null)
        {
            foreach (GameObject lightObject in lightsToTurnOn)
            {
                if (lightObject != null)
                    lightObject.SetActive(true);
            }
        }

        if (music != null)
            music.Play();

        if (cube3 != null)
            cube3.SetActive(true);

        if (generatorPanel != null)
            generatorPanel.SetActive(false);

        panelOpen = false;


        // Enable camera look again
        if (cameraLook != null)
        {
            cameraLook.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Generator completed!");
    }


    // =========================================================
    // BUTTON COLOR
    // =========================================================

    void SetButtonColor(Button button, Color color)
    {
        if (button == null)
            return;

        Image image = button.GetComponent<Image>();

        if (image != null)
            image.color = color;
    }
}