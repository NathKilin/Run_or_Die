using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameOver = false;

    [Header("References")]
    public GameObject gameOverUI; 
void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // when player´s health reaches zero    
    public void GameOver()
    {
    if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Manager: GAME OVER INITIALIZED");
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.StopScoring();

        
        if (gameOverUI != null)
        { 
            gameOverUI.SetActive(true);
        }
        
        Time.timeScale = 0f;

    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}