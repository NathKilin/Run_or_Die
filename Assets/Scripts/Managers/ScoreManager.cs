using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Settings")] [SerializeField]
    private Transform player;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private DifficultyProfile difficultyProfile;

    public float startY;
    public float currentScore;
    public float scoreBonus = 0;
    private bool isScoring = true;
    public float bestDistance = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (player != null) {
            startY = player.position.y;
        }

        if (difficultyProfile == null) {
            Debug.LogWarning("Playing the default difficulty: normal.");
        }

        bestDistance = 0f;
        currentScore = 0f;
        UpdateUI();
    }

    void Update()
    {
        if (!isScoring || player == null) return;

        float distance = player.position.y - startY;

        if (distance > bestDistance) {
            bestDistance = distance;

            int multiplier = (difficultyProfile != null) ? difficultyProfile.scoreMultiplier : 10;

            currentScore = bestDistance * multiplier + scoreBonus;
        }

        UpdateUI();
    }



    void UpdateUI()
    {
        if (scoreText != null) {
            scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
        }

    }


    public void StopScoring()
    {
        isScoring = false;

    }

    
    public void SetDifficulty(DifficultyProfile profile)
    {
        difficultyProfile = profile;
    }
}