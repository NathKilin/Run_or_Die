using UnityEngine;
using UnityEngine.UI;   

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;  
    public Slider healthSlider;       

    private void Start()
    {

        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.minValue = 0;

        healthSlider.value = playerHealth.CurrentHealth;

    }

    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;    
        }
    }
}
