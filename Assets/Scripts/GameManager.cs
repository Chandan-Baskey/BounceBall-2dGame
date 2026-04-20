using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int score;
    public Text textScore;
    public GameObject gameStartPanel;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        gameStartPanel.SetActive(false);
        textScore.gameObject.SetActive(true);
    }
}
