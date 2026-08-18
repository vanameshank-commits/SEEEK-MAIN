using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string introSceneName = "IntroScene";
    public string mainMenuSceneName = "Main menu";
    public string controlPanelSceneName = "ControlPanel";
    public string gameSceneName = "SEEK-MAIN";

    [Header("Timing")]
    public float introTime = 3f;
    public float controlPanelTime = 3f;

    private static MainMenuManager instance;

    private void Awake()
    {
        // Only one manager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CheckCurrentScene();
    }

    private void CheckCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log("Current Scene: " + currentScene);

        // INTRO
        if (currentScene == introSceneName)
        {
            Debug.Log("INTRO STARTED - 3 SECOND TIMER");

            CancelInvoke();
            Invoke(nameof(LoadMainMenu), introTime);
        }

        // CONTROL PANEL
        else if (currentScene == controlPanelSceneName)
        {
            Debug.Log("CONTROL PANEL STARTED - 3 SECOND TIMER");

            CancelInvoke();
            Invoke(nameof(LoadGame), controlPanelTime);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckCurrentScene();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // INTRO → MAIN MENU
    private void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // PLAY → CONTROL PANEL
    public void PlayGame()
    {
        Debug.Log("PLAY BUTTON PRESSED");

        CancelInvoke();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(controlPanelSceneName);
    }

    // CONTROL PANEL → GAME
    private void LoadGame()
    {
        Debug.Log("Loading SEEK-MAIN...");

        SceneManager.LoadScene(gameSceneName);
    }

    // CREDITS
    public void OpenCredits()
    {
        GameObject credits = GameObject.Find("Credits Panel");

        if (credits != null)
            credits.SetActive(true);
        else
            Debug.LogWarning("Credits Panel not found!");
    }

    public void CloseCredits()
    {
        GameObject credits = GameObject.Find("Credits Panel");

        if (credits != null)
            credits.SetActive(false);
    }

    // QUIT
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}