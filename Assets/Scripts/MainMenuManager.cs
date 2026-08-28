using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string introSceneName = "IntroScene";
    public string mainMenuSceneName = "Main menu";
    public string controlPanelSceneName = "ControlPanel";
    public string storySceneName = "StoryScene";
    public string gameSceneName = "SEEK-MAIN";

    [Header("Timing")]
    public float introTime = 3f;
    public float controlPanelTime = 3f;
    public float storyTime = 8f;

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

        // INTRO → MAIN MENU
        if (currentScene == introSceneName)
        {
            Debug.Log("INTRO STARTED - 3 SECOND TIMER");

            CancelInvoke();
            Invoke(nameof(LoadMainMenu), introTime);
        }

        // CONTROL PANEL → STORY
        else if (currentScene == controlPanelSceneName)
        {
            Debug.Log("CONTROL PANEL STARTED - 3 SECOND TIMER");

            CancelInvoke();
            Invoke(nameof(LoadStoryScene), controlPanelTime);
        }

        // STORY → GAME
        else if (currentScene == storySceneName)
        {
            Debug.Log("STORY SCENE STARTED - 8 SECOND TIMER");

            CancelInvoke();
            Invoke(nameof(LoadGame), storyTime);
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


    // =========================================================
    // INTRO → MAIN MENU
    // =========================================================

    private void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");

        SceneManager.LoadScene(mainMenuSceneName);
    }


    // =========================================================
    // PLAY → CONTROL PANEL
    // =========================================================

    public void PlayGame()
    {
        Debug.Log("PLAY BUTTON PRESSED");

        CancelInvoke();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(controlPanelSceneName);
    }


    // =========================================================
    // CONTROL PANEL → STORY
    // =========================================================

    private void LoadStoryScene()
    {
        Debug.Log("Loading Story Scene...");

        SceneManager.LoadScene(storySceneName);
    }


    // =========================================================
    // STORY → GAME
    // =========================================================

    private void LoadGame()
    {
        Debug.Log("Loading SEEK-MAIN...");

        SceneManager.LoadScene(gameSceneName);
    }


    // =========================================================
    // CREDITS
    // =========================================================

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


    // =========================================================
    // QUIT
    // =========================================================

    //
}