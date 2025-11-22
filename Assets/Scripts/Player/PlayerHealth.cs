using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Difficulty Settings")]
    [SerializeField] private DifficultyProfile difficultyProfile;

    [Header("Debug Info")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    public UnityEvent<int> onHealthChanged; 
    public UnityEvent onDamaged;
    public UnityEvent onDied;

    public bool IsDead => currentHealth <= 0;   
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        if (difficultyProfile != null)
        {
            maxHealth = difficultyProfile.maxHealth;
            currentHealth = maxHealth;
        }
        else
        {
            Debug.LogWarning("Nenhum DifficultyProfile associado! Usando valores padrão.");
            maxHealth = 3;
            currentHealth = 3;
        }
        
        onHealthChanged?.Invoke(currentHealth);
    }

    public bool TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
            return false;

        currentHealth -= amount;

        onDamaged?.Invoke();
        onHealthChanged?.Invoke(currentHealth);

        if (IsDead)
            Die();

        return true;
    }

    private void Die()
    {
        if (currentHealth > 0) currentHealth = 0; 
        
        onDied?.Invoke();
        Debug.Log("Player has died!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }
}