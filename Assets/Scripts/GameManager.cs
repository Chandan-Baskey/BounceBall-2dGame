using System.Collections;
using System.Collections.Generic;
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


    private void Awake()
    {
        instance = this; // Set the singleton instance
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
        rightTab.SetActive(false);
        leftTab.SetActive(false);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }



}
