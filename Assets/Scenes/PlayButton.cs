using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        MainMenuManager manager = FindFirstObjectByType<MainMenuManager>();

        if (manager != null)
        {
            manager.PlayGame();
        }
        else
        {
            Debug.LogError("MainMenuManager not found!");
        }
    }


    // =========================================================
    // QUIT GAME
    // =========================================================

    public void Quit()
    {
        Debug.Log("QUIT BUTTON PRESSED");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}