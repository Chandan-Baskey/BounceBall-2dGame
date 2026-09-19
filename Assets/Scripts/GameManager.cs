using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance for easy access from other scripts
    int score;
    [SerializeField] private Text textScore;
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject rightTab;
    [SerializeField] private GameObject leftTab;
    [SerializeField] private GameObject back;

    public bool IsPlaying { get; private set; }
    public bool PointerReleaseRequired { get; private set; }

    private void Awake()
    {
        instance = this; // Set the singleton instance
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore()
    {
        score++;
        //textScore.text = "Score: " + score;
        textScore.text = score.ToString();
    }
    public void GameStart()
    {
        if (IsPlaying)
        {
            return;
        }

        IsPlaying = true;
        // Do not let the pointer that starts the round also move the paddle.
        PointerReleaseRequired = true;
        gameStartPanel.SetActive(false);
        textScore.gameObject.SetActive(true);
        rightTab.SetActive(false);
        leftTab.SetActive(false);
        back.SetActive(true);

    }
    public void NotifyPointersReleased()
    {
        PointerReleaseRequired = false;
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
