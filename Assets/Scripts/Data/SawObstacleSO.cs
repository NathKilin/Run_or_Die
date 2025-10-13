using UnityEngine;

/*
 * SawObstacleSO
 * -------------
 * DATA ONLY that describes a saw obstacle's motion and look.
 * You can make multiple assets:
 *   - Saw_MidTrack (e.g., horizontal sweep in the middle)
 *   - Saw_SideLeft / Saw_SideRight (vertical up/down along the side)
 *
 * This does NOT contain code for collisions or physics. It's just numbers/settings.
 */

namespace RunOrDie
{
    public enum SawKind { MidTrack, Side }

    [CreateAssetMenu(menuName = "RunOrDie/Saw Obstacle")]
    public class SawObstacleSO : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Just a label so designers know what this asset is for.")]
        public string displayName = "Saw";

        [Tooltip("Helps remember intended placement/type.")]
        public SawKind kind = SawKind.MidTrack;

        [Header("Movement (oscillation)")]
        [Tooltip("If true, the saw will move back and forth along 'moveAxis'.")]
        public bool oscillate = true;

        [Tooltip("Direction of movement: (1,0)=horizontal, (0,1)=vertical, etc.")]
        public Vector2 moveAxis = Vector2.up;

        [Tooltip("How far from the center it moves (peak). e.g., 2 means +2 / -2 units.")]
        public float amplitude = 2f;

        [Tooltip("How fast it oscillates: cycles per second (1 = one full back&forth per second).")]
        public float moveSpeed = 1.5f;

        [Header("Rotation (spin)")]
        [Tooltip("If true, the saw will spin around Z.")]
        public bool rotate = true;

        [Tooltip("Degrees per second around Z axis.")]
        public float rotationSpeed = 180f;
    }
}
