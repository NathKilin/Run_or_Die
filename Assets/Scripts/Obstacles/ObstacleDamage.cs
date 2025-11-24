using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damageAmount = 1; 
    private bool hasTriggered = false; 



    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) 
            return;

        hasTriggered = true;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            Destroy(gameObject);
        }
    }
}
