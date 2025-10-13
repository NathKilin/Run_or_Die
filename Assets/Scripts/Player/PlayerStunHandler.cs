using UnityEngine;

/*
 * PlayerStunHandler
 * -----------------
 * Default implementation of IStunnable for the Player.
 * On Stun():
 *   - temporarily disables your movement scripts
 *   - cancels upward velocity
 *   - increases gravity (optional) so the player falls
 *   - after a short duration, re-enables control (if you didn't die)
 *
 * NOTE: Drag your movement scripts into 'inputScriptsToDisable' in the Inspector.
 * If you don't have any yet, you can leave it empty and the player will still fall.
 */

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStunHandler : MonoBehaviour, IStunnable
{
    [Header("What to disable while stunned")]
    [Tooltip("Add your Player movement/controller scripts here (MonoBehaviours).")]
    [SerializeField] private MonoBehaviour[] inputScriptsToDisable;

    [Header("Stun behavior (feel)")]
    [Tooltip("Gravity scale while stunned. Higher = faster fall.")]
    [SerializeField] private float stunnedGravityScale = 2.5f;

    [Tooltip("Seconds until controls re-enable (if the player didn't die).")]
    [SerializeField] private float stunDuration = 1.2f;

    private Rigidbody2D rb;
    private float originalGravity;
    private bool isStunned;
    private float stunTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    [System.Obsolete]
    public void Stun()
    {
        if (isStunned) return;
        isStunned = true;
        stunTimer = stunDuration;

        // Disable input/movement scripts
        foreach (var m in inputScriptsToDisable)
            if (m) m.enabled = false;

        // Cancel upward momentum and let gravity pull us down
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.gravityScale = stunnedGravityScale;

    }

    private void Update()
    {
        if (!isStunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            // Re-enable control (unless a death script already destroyed this object)
            foreach (var m in inputScriptsToDisable)
                if (m) m.enabled = true;

            rb.gravityScale = originalGravity;
            isStunned = false;
        }
    }
}
