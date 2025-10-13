using UnityEngine;

/*
 * SawFromSO
 * ---------
 * PURPOSE:
 *   - Reads a SawObstacleSO asset to animate a saw (spin + oscillate).
 *   - On collision with the player, tells the player to "Stun()" — no physics shove.
 *
 * REQUIREMENTS ON THE PREFAB:
 *   - A 2D collider (Box/Circle/Polygon) set to NOT trigger (solid hazard).
 *   - Visuals (sprite/mesh) as you like.
 *
 * HOW IT WORKS:
 *   - Stores start position in Awake()
 *   - Each Update() applies rotation and sinusoidal movement if enabled by the SO
 *   - OnCollisionEnter2D: if we hit the player, try to find an IStunnable and call Stun()
 */

namespace RunOrDie
{
    [RequireComponent(typeof(Collider2D))]
    public class SawFromSO : MonoBehaviour
    {
        [Header("Data")]
        [Tooltip("Drop a SawObstacleSO here (e.g., Saw_MidTrack, Saw_SideLeft).")]
        [SerializeField] private SawObstacleSO def;

        [Header("Who counts as the player")]
        [Tooltip("Only GameObjects with this tag will be stunned on contact.")]
        [SerializeField] private string playerTag = "Player";

        // Internal state for movement
        private Vector3 basePos; // where the saw started (center of its oscillation)
        private float timeAccumulator; // used to advance the sine wave

        private void Awake()
        {
            basePos = transform.position;

            // Ensure collider exists and is solid (not a trigger), since this is a hazard
            var col = GetComponent<Collider2D>();
            if (col) col.isTrigger = false;
        }

        private void Update()
        {
            if (def == null) return;

            // 1) Rotation (spin around Z)
            if (def.rotate)
            {
                float dz = def.rotationSpeed * Time.deltaTime;
                transform.Rotate(0f, 0f, dz, Space.Self);
            }

            // 2) Oscillation (move along a chosen axis using a sine wave)
            if (def.oscillate)
            {
                // Move 'moveSpeed' cycles per second → convert to radians per second
                float radiansPerSec = def.moveSpeed * Mathf.PI * 2f;
                timeAccumulator += radiansPerSec * Time.deltaTime;

                // Sin ranges -1..+1 → multiply by amplitude → offset along moveAxis
                Vector2 axis = def.moveAxis.sqrMagnitude > 0f ? def.moveAxis.normalized : Vector2.up;
                Vector2 offset = axis * (Mathf.Sin(timeAccumulator) * def.amplitude);

                transform.position = basePos + (Vector3)offset;
            }
            else
            {
                // If not oscillating, keep the saw pinned at its base position (useful if you toggle at runtime)
                transform.position = basePos;
            }
        }

        private void OnCollisionEnter2D(Collision2D c)
        {
            // Only react to the player (by tag)
            if (!c.collider.CompareTag(playerTag)) return;

            // Try to get an IStunnable on the player or its parents
            if (c.collider.TryGetComponent<IStunnable>(out var stunnable))
            {
                stunnable.Stun();
                return;
            }

            var stunOnParent = c.collider.GetComponentInParent<IStunnable>();
            stunOnParent?.Stun();
        }
    }
}
