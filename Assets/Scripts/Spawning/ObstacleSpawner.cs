using UnityEngine;

/*
 * ObstacleSpawner
 * ---------------
 * PURPOSE:
 *   Continuously spawn obstacles above the player as they climb.
 *   Uses:
 *     - SpawnTableSO: to pick WHICH rule to spawn (weighted + height-gated)
 *     - ObstacleSpawnRuleSO: to know prefab, spacing, and X placement
 *     - IObstacleFactory: to actually create the object (Instantiate or pooled)
 *
 * HOW IT WORKS:
 *   - We keep 'nextSpawnY' as the Y at which we'll place the next obstacle.
 *   - Every frame, we ensure the world is populated up to playerY + maxLookahead.
 *   - After we spawn one obstacle, we advance nextSpawnY by a random gap
 *     between ySpacingMin and ySpacingMax from the chosen rule.
 *
 * SETUP IN INSPECTOR:
 *   - player: your Player root Transform (we read its Y).
 *   - table: a SpawnTableSO with your rules (Mid, SideLeft, SideRight, ...).
 *   - factoryBehaviour: drag a component that implements IObstacleFactory
 *       (SimpleInstantiateFactory for now).
 *   - parentForSpawns: optional container Transform for hierarchy cleanliness.
 *   - startY: first spawn height (relative to world, not player).
 *   - maxLookahead: how far above the player we keep spawning.
 */

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private SpawnTableSO table;

    [Tooltip("Assign a component that implements IObstacleFactory (e.g., SimpleInstantiateFactory).")]
    [SerializeField] private MonoBehaviour factoryBehaviour; // we cast this to IObstacleFactory at runtime
    private IObstacleFactory factory;

    [Header("Hierarchy")]
    [Tooltip("Optional parent to keep spawned obstacles organized in the Hierarchy.")]
    [SerializeField] private Transform parentForSpawns;

    [Header("Spawn band")]
    [Tooltip("World Y where the first obstacle will be spawned.")]
    [SerializeField] private float startY = 8f;

    [Tooltip("How far above the player we make sure obstacles exist.")]
    [SerializeField] private float maxLookahead = 30f;

    // Internal state: the Y coordinate for the next spawn
    private float nextSpawnY;

    private void Awake()
    {
        // Convert the MonoBehaviour into the interface (will be null if not implementing)
        factory = factoryBehaviour as IObstacleFactory;

        if (factory == null)
            Debug.LogError("ObstacleSpawner: 'factoryBehaviour' must implement IObstacleFactory.");

        if (!parentForSpawns) parentForSpawns = this.transform;

        nextSpawnY = startY;
    }

    private void Update()
    {
        if (player == null || table == null || factory == null) return;

        // We want content up to playerY + maxLookahead
        float targetFillY = player.position.y + maxLookahead;

        // Keep spawning until we've filled that band
        while (nextSpawnY <= targetFillY)
        {
            float atHeight = nextSpawnY;

            // Pick a rule eligible for this height
            var rule = table.Pick(atHeight);
            if (rule == null)
            {
                // If nothing eligible, nudge nextSpawnY to avoid infinite loop.
                nextSpawnY += 4f;
                continue;
            }

            // Decide X position
            float x = 0f;
            if (rule.lockToSide)
            {
                // For side-locked rules we use:
                //  LEFT  -> xRange.x
                //  RIGHT -> xRange.y
                //  CENTER-> 0 (change if you want a specific center X)
                switch (rule.lane)
                {
                    case Lane.Left:  x = rule.xRange.x; break;
                    case Lane.Right: x = rule.xRange.y; break;
                    default:         x = 0f;            break;
                }
            }
            else
            {
                // Random X in range
                x = Random.Range(rule.xRange.x, rule.xRange.y);
            }

            // Spawn the obstacle
            Vector3 pos = new Vector3(x, nextSpawnY, 0f);
            factory.Spawn(rule.obstaclePrefab, pos, Quaternion.identity, parentForSpawns);

            // Advance next Y by rule spacing (at least 1 unit for safety)
            float gap = Random.Range(rule.ySpacingMin, rule.ySpacingMax);
            nextSpawnY += Mathf.Max(1f, gap);
        }
    }
}
