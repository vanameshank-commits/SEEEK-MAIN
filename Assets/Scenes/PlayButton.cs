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
}