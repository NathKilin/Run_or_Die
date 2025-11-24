using UnityEngine;
using UnityEngine.UI;   

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;  
    public Slider healthSlider;       

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("[HealthBarUI] Missing PlayerHealth reference!");
            return;
        }

        if (healthSlider == null)
        {
            Debug.LogWarning("[HealthBarUI] Missing Slider reference!");
            return;
        }

        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.minValue = 0;
        healthSlider.value   = playerHealth.CurrentHealth;


        playerHealth.onHealthChanged.AddListener(UpdateHealth);
    }

    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;    
        }
    }
}
