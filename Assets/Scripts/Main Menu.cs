using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Main Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    } 
}
