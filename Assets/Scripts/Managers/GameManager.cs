using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    [HideInInspector] public bool isGameOver = false;

    [Header("References")]
    [SerializeField] private Canvas gameOverUI; 
    [SerializeField] private VerticalLayoutGroup statisticsContainer;
    
    [HideInInspector] public int timesJumped = 0;
    [HideInInspector] public int timesDashed = 0;
    
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        gameOverUI.gameObject.SetActive(false);
        
        timesJumped = 0;
        timesDashed = 0;
    }

    // when player´s health reaches zero 
    public void GameOver()
    {
         if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        
        ScoreManager.Instance.StopScoring();
        
        gameOverUI.gameObject.SetActive(true);
        SetStatistics();

    }


    private void SetStatistics()
    {
        // Destroy children visible in editor 
        for (int i = 0; i < statisticsContainer.transform.childCount; i++) {
            GameObject child = statisticsContainer.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        CreateNewStatistic($"Score : {(int)ScoreManager.Instance.currentScore}");
        CreateNewStatistic($"Times Jumped : {timesJumped}");
        CreateNewStatistic($"Times Dashed : {timesDashed}");
    }


    private void CreateNewStatistic(string message, int fontSize = 125)
    {
        GameObject obj = new GameObject("StatisticLabel");
        TextMeshProUGUI statisticLabel = obj.AddComponent<TextMeshProUGUI>();
        
        statisticLabel.fontSize = fontSize;
        statisticLabel.text = "<b>"+message;
        statisticLabel.alignment = TextAlignmentOptions.Center;
        statisticLabel.textWrappingMode = TextWrappingModes.NoWrap;
        
        Instantiate(statisticLabel);
        statisticLabel.transform.SetParent(statisticsContainer.transform,false);
    }
    
    
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    
    
    
    
    
    
    
    
    
    
}