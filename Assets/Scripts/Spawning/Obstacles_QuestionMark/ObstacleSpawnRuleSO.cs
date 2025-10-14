using UnityEngine;

/*
 * ObstacleSpawnRuleSO
 * -------------------
 * DATA ONLY for a single obstacle spawn rule.
 *
 * Typical usage:
 *   - Make a rule for Mid-Track Saw (random X within xRange).
 *   - Make rules for Side Saw (lockToSide = true) using xRange.x for LEFT fixed X
 *     and xRange.y for RIGHT fixed X (or set both to same number if you want).
 *
 * The spawner will:
 *   - Choose a rule via SpawnTableSO.Pick(height)
 *   - Compute an X based on lockToSide/lane/xRange
 *   - Spawn the given prefab
 */

public enum Lane { Center = 0, Left = -1, Right = 1 }

[CreateAssetMenu(menuName = "RunOrDie/Spawning/Obstacle Rule")]
public class ObstacleSpawnRuleSO : ScriptableObject
{
    [Header("What to spawn")]
    [Tooltip("The prefab to instantiate (e.g., a Saw prefab with SawFromSO).")]
    public GameObject obstaclePrefab;

    [Header("Probability")]
    [Tooltip("Relative chance to be selected. 2 = twice as likely as 1.")]
    public float weight = 1f;

    [Header("Where it can appear (X placement)")]
    [Tooltip("If true, the X is fixed to a side lane. If false, X is random in xRange.")]
    public bool lockToSide = false;

    [Tooltip("Which side lane if lockToSide is true. Center is allowed too.")]
    public Lane lane = Lane.Center;

    [Tooltip("If NOT locked to side: random X between xRange.x and xRange.y.\n" +
             "If locked to side: we use xRange.x for LEFT, xRange.y for RIGHT, and 0 for CENTER unless you change it.")]
    public Vector2 xRange = new Vector2(-1.5f, 1.5f);

    [Header("Vertical spacing (distance to next spawn)")]
    [Tooltip("Min vertical gap after this spawn.")]
    public float ySpacingMin = 6f;

    [Tooltip("Max vertical gap after this spawn.")]
    public float ySpacingMax = 10f;

    [Header("Height gating (difficulty ramp)")]
    [Tooltip("Only eligible if current height >= minHeight.")]
    public float minHeight = 0f;

    [Tooltip("Only eligible if current height <= maxHeight.")]
    public float maxHeight = 9999f;
}
