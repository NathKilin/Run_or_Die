using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "RunOrDie/Difficulty Profile")]
public class DifficultyProfile : ScriptableObject
{
    [Header("Difficultuy Settings")]
    public string difficultyName = "Normal";

    public int maxHealth = 3;
    public int obstacleDamage = 1;
    public float obstacleSpawnRate = 2f;
    
    [Header("Score")]
    public int scoreMultiplier = 10;
}