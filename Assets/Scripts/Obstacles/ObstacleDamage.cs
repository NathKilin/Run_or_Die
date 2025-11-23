using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damageAmount = 1; 

    private void Start()
    {
        // Se existir DifficultyManager, usamos o dano do profile atual
        if (DifficultyManager.Instance != null &&
            DifficultyManager.Instance.currentProfile != null)
        {
            var profile = DifficultyManager.Instance.currentProfile;
            damageAmount = profile.obstacleDamage;

            Debug.Log($"[ObstacleDamage] Damage = {damageAmount} (" +
                      profile.difficultyName + ")");
        }
    }

    private void HandleDamage(GameObject target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleDamage(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleDamage(other.gameObject);
    }
}
