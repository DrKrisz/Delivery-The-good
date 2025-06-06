using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void Options()
    {
        SceneManager.LoadScene("Options");
    }
    public void QuitGame()
    {
        SaveSystem system = FindObjectOfType<SaveSystem>();
        if (system != null)
            system.SaveGame();
        Application.Quit();
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
