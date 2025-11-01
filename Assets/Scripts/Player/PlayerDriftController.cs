using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDriftController : MonoBehaviour
{
    [Header("Anchors (fixed)")]
    public float anchorY = -2.5f;
    public float anchorZ =  2.0f;

    [Header("Horizontal Limits")]
    public float leftLimitX  = -2.5f;
    public float rightLimitX =  5.5f;

    [Header("Drift")]
    [Tooltip("Horizontal drift speed while Space is held.")]
    public float driftSpeed = 2.25f;

    [Tooltip("Initial horizontal direction: -1 = left, +1 = right")]
    public int startDir = +1;

    [Header("World Scroll")]
    [Tooltip("Reference to your WallPool (assign in Inspector).")]
    public WallPool wallPool;

    [Tooltip("Upward climb speed when Space is held (usually equals WallPool.speed).")]
    public float climbSpeed = 5f;

    [Tooltip("Downward fall speed when Space is released (scenario moves up).")]
    public float fallSpeed = 4f;

    [Header("Grounding")]
    [Tooltip("Which layers count as 'platforms' to rest on.")]
    public LayerMask platformMask;

    [Tooltip("Half-extent of the foot probe box on XZ. Keep tiny.")]
    public Vector2 footProbeHalfSize = new Vector2(0.35f, 0.35f);

    [Tooltip("Distance below anchorY to check for a platform surface.")]
    public float footProbeDepth = 0.15f;

    [Header("Collision")]
    [Tooltip("Layers that are considered lethal (e.g., spinning blades).")]
    public LayerMask lethalMask;

    int _dir;
    bool _spaceHeld;

    void Awake()
    {
        _dir = Mathf.Sign(startDir) >= 0 ? +1 : -1;
    }

    void Start()
    {
        // Snap to anchor (Y/Z)
        Vector3 p = transform.position;
        p.y = anchorY; p.z = anchorZ;
        transform.position = p;

        if (!wallPool)
            Debug.LogWarning("[PlayerDriftController] WallPool reference missing! Assign it in the Inspector.");
    }

    void Update()
    {
        // --- 1) Read input
        _spaceHeld = Input.GetKey(KeyCode.Space);

        // --- 2) Horizontal drift when Space is held
        Vector3 pos = transform.position;

        if (_spaceHeld)
        {
            pos.x += _dir * driftSpeed * Time.deltaTime;

            // Edge flip
            if (pos.x >= rightLimitX) { pos.x = rightLimitX; _dir = -1; }
            if (pos.x <= leftLimitX)  { pos.x = leftLimitX;  _dir = +1; }
        }

        // --- 3) Hard-anchor Y/Z every frame
        pos.y = anchorY;
        pos.z = anchorZ;
        transform.position = pos;

        // --- 4) Determine world scroll based on input & ground
        bool grounded = IsStandingOnPlatform();

        if (_spaceHeld)
        {
            // climbing illusion: world moves DOWN (+)
            wallPool?.SetCurrentSpeed(climbSpeed);
        }
        else
        {
            if (grounded)
            {
                // only truly stop if resting on a platform
                wallPool?.SetCurrentSpeed(0f);
            }
            else
            {
                // falling illusion: world moves UP (-)
                wallPool?.SetCurrentSpeed(-Mathf.Abs(fallSpeed));
            }
        }

        // --- 5) Lethal overlap (blades etc.)
        if (TouchesLethal())
        {
            OnPlayerDeath();
        }
    }

    bool IsStandingOnPlatform()
    {
        // Probe a tiny box just under the anchor feet
        Vector3 center = new Vector3(transform.position.x,
                                     anchorY - footProbeDepth * 0.5f,
                                     anchorZ);
        Vector3 halfExt = new Vector3(footProbeHalfSize.x, footProbeDepth * 0.5f, footProbeHalfSize.y);

        Collider[] hits = Physics.OverlapBox(center, halfExt, Quaternion.identity, platformMask, QueryTriggerInteraction.Ignore);
        return hits != null && hits.Length > 0;
    }

    bool TouchesLethal()
    {
        // Small overlap at player capsule center to catch blade contact
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.4f, lethalMask, QueryTriggerInteraction.Ignore);
        return hits != null && hits.Length > 0;
    }

    void OnDrawGizmosSelected()
    {
        // visualize foot probe
        Gizmos.color = Color.green;
        Vector3 c = new Vector3(transform.position.x, anchorY - footProbeDepth * 0.5f, anchorZ);
        Vector3 s = new Vector3(footProbeHalfSize.x * 2f, footProbeDepth, footProbeHalfSize.y * 2f);
        Gizmos.DrawWireCube(c, s);
    }

    void OnPlayerDeath()
    {
        Debug.LogWarning("[PlayerDriftController] Lethal contact!");
        // TODO: trigger death, reset, or damage
        // For now, halt scroll to make it obvious:
        wallPool?.SetCurrentSpeed(0f);
    }
}
