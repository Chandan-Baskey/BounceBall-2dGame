using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private const string GameplaySceneName = "LvL";

    public void StartGame()
    {
        SceneManager.LoadScene(GameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
