using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damageAmount = 1;

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