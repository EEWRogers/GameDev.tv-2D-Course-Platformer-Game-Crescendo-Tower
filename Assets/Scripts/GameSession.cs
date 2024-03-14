using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    ScenePersist scenePersist;
    int playerScore = 0;

    void Awake() 
    {
        scenePersist = FindObjectOfType<ScenePersist>();

        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numberOfGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }

        livesText.text = "Lives: " + playerLives.ToString();
        scoreText.text = "Score: " + playerScore.ToString(); 
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void TakeLife()
    {
        playerLives--;

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        livesText.text = "Lives: " + playerLives.ToString();
    }

    public void IncreaseScore(int scoreValue)
    {
        playerScore += scoreValue;
        scoreText.text = "Score: " + playerScore.ToString(); 
    }

    void ResetGameSession()
    {
        scenePersist.ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
