using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public GameObject EImage;


    public GameObject panel;

    public TMP_InputField inputField;

    public string correctCode = "1234";

    public GameObject walt;

    public GameObject key;

    bool near;

    bool panelOpen;

    void Start()
    {
        if (EImage != null)
            EImage.SetActive(false);

        panel.SetActive(false);

        key.SetActive(false);
    }

    void Update()
    {
        if (
            near
            &&
            !panelOpen
        )
        {
            if (EImage != null)
                EImage.SetActive(true);

            if (
                Input.GetKeyDown(
                    KeyCode.E
                )
            )
            {
                OpenPanel();
            }
        }

        if (
            panelOpen
            &&
            Input.GetKeyDown(
                KeyCode.Q
            )
        )
        {
            ClosePanel();
        }
    }

    void OpenPanel()
    {
        panelOpen = true;

        panel.SetActive(true);

        if (EImage != null)
            EImage.SetActive(false);

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible =
            true;
    }

    void ClosePanel()
    {
        panelOpen = false;

        panel.SetActive(false);

        inputField.text = "";

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible =
            false;
    }

    public void AddNumber(
    string number
)
    {
        if (
            inputField.text.Length
            <
            correctCode.Length
        )
        {
            inputField.text +=
                number;
        }
    }

    public void ClearInput()
    {
        inputField.text = "";
    }

    public void EnterCode()
    {
        if (
        inputField.text
        ==
        correctCode
        )
        {
            Debug.Log(
            "Correct Code"
            );

            walt.SetActive(
                false
            );

            key.SetActive(
                true
            );

            ClosePanel();
        }

        else
        {
            Debug.Log(
                "Wrong Code"
            );

            inputField.text =
                "";
        }

    }

    void OnTriggerEnter(
        Collider other
    )
    {
        if (
            other.CompareTag(
                "Player"
            )
        )
        {
            near = true;
        }
    }

    void OnTriggerExit(
        Collider other
    )
    {
        if (
            other.CompareTag(
                "Player"
            )
        )
        {
            near = false;

            if (EImage != null)
                EImage.SetActive(false);

            ClosePanel();
        }
    }


}
