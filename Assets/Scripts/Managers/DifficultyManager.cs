using UnityEngine;

// Este script guarda qual dificuldade está ativa agora
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;   // acesso global simples

    [Header("Profiles disponíveis")]
    public DifficultyProfile easyProfile;
    public DifficultyProfile hardProfile;

    [Header("Current Profile")]
    public DifficultyProfile currentProfile;

    [Header("Scene References")]
    public PlayerHealth playerHealth;   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (currentProfile == null && easyProfile != null)
        {
            currentProfile = easyProfile;
        }

        ApplyCurrentToPlayer();
    }

    public void ToggleDifficulty()
    {
        if (currentProfile == easyProfile && hardProfile != null)
        {
            currentProfile = hardProfile;
        }
        else if (easyProfile != null)
        {
            currentProfile = easyProfile;
        }

        Debug.Log("[DifficultyManager] Difficulty now: " + currentProfile.difficultyName);

        ApplyCurrentToPlayer();
    }

    public void ApplyCurrentToPlayer()
    {
        if (playerHealth == null || currentProfile == null) return;

        playerHealth.ApplyDifficulty(currentProfile);
    }
}
