using UnityEngine;
using TMPro;

public class Locker : MonoBehaviour
{
    [Header("References")]
    public GameObject keypadPanel;
    public Animator lockerAnimator;

    // TWO OBJECTS INSIDE LOCKER
    public GameObject objectInside1;
    public GameObject objectInside2;

    public AudioSource openSound;

    [Header("Player Control")]
    public PlayerMovement playerController;
    public MouseLook mouseLook;

    [Header("Code Display")]
    public TMP_Text codeDisplay;

    [Header("Password")]
    public string correctCode = "1234";

    private string enteredCode = "";
    private bool keypadOpen = false;
    private bool unlocked = false;


    void Start()
    {
        if (keypadPanel != null)
            keypadPanel.SetActive(false);

        if (objectInside1 != null)
            objectInside1.SetActive(false);

        if (objectInside2 != null)
            objectInside2.SetActive(false);

        if (codeDisplay != null)
            codeDisplay.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        // Close keypad with Q
        if (keypadOpen && Input.GetKeyDown(KeyCode.Q))
        {
            CloseKeypad();
        }
    }


    // =========================================================
    // OPEN KEYPAD
    // =========================================================

    public void OpenKeypad()
    {
        if (unlocked)
            return;

        keypadOpen = true;
        enteredCode = "";

        if (keypadPanel != null)
            keypadPanel.SetActive(true);

        if (codeDisplay != null)
            codeDisplay.text = "";

        // Lock player movement
        if (playerController != null)
            playerController.enabled = false;

        // Lock camera
        if (mouseLook != null)
            mouseLook.canLook = false;

        // Show cursor for keypad
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Keypad opened.");
    }


    // =========================================================
    // ADD NUMBER
    // =========================================================

    public void AddNumber(string number)
    {
        if (!keypadOpen)
            return;

        if (enteredCode.Length >= correctCode.Length)
            return;

        enteredCode += number;

        if (codeDisplay != null)
            codeDisplay.text = enteredCode;

        Debug.Log("Entered: " + enteredCode);
    }


    // =========================================================
    // CLEAR
    // =========================================================

    public void ClearCode()
    {
        if (!keypadOpen)
            return;

        enteredCode = "";

        if (codeDisplay != null)
            codeDisplay.text = "";

        Debug.Log("Code cleared.");
    }


    // =========================================================
    // ENTER CODE
    // =========================================================

    public void EnterCode()
    {
        if (!keypadOpen)
            return;

        if (enteredCode == correctCode)
        {
            UnlockLocker();
        }
        else
        {
            Debug.Log("Wrong code!");

            enteredCode = "";

            if (codeDisplay != null)
                codeDisplay.text = "";
        }
    }


    // =========================================================
    // UNLOCK
    // =========================================================

    void UnlockLocker()
    {
        unlocked = true;
        keypadOpen = false;

        // Hide keypad
        if (keypadPanel != null)
            keypadPanel.SetActive(false);

        if (codeDisplay != null)
            codeDisplay.text = "";

        // Enable player
        if (playerController != null)
            playerController.enabled = true;

        // Enable camera
        if (mouseLook != null)
            mouseLook.canLook = true;

        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Play sound
        if (openSound != null)
            openSound.Play();

        // Play opening animation
        if (lockerAnimator != null)
            lockerAnimator.SetTrigger("Open");

        // Show BOTH objects after animation
        Invoke(nameof(ShowObjects), 4f);

        Debug.Log("Correct code! Locker opened.");
    }


    // =========================================================
    // CLOSE KEYPAD WITH Q
    // =========================================================

    void CloseKeypad()
    {
        keypadOpen = false;
        enteredCode = "";

        if (keypadPanel != null)
            keypadPanel.SetActive(false);

        if (codeDisplay != null)
            codeDisplay.text = "";

        EnablePlayerControl();

        Debug.Log("Keypad closed.");
    }


    // =========================================================
    // ENABLE PLAYER CONTROL
    // =========================================================

    void EnablePlayerControl()
    {
        if (playerController != null)
            playerController.enabled = true;

        if (mouseLook != null)
            mouseLook.canLook = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // =========================================================
    // SHOW BOTH OBJECTS
    // =========================================================

    void ShowObjects()
    {
        if (objectInside1 != null)
            objectInside1.SetActive(true);

        if (objectInside2 != null)
            objectInside2.SetActive(true);

        Debug.Log("Both objects inside locker are available.");
    }
}