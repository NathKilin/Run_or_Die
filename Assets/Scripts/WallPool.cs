using UnityEngine;

public class WallPool : MonoBehaviour
{
    [Header("Segments (exactly 4, ordered doesn't matter)")]
    public Transform[] segments;

    [Header("Player to track")]
    [SerializeField] private Transform player;

    [Header("Segment size")]
    [Tooltip("If 0, we will measure the height from the first segment renderer.")]
    public float segmentHeight = 0f;

    [Header("How close to the edge before we swap")]
    [Range(0.05f, 0.5f)]
    public float edgePercent = 0.2f;

    private float _height;

    void Start()
    {
        _height = (segmentHeight > 0f) ? segmentHeight : MeasureHeight();

        if (player == null)
        {
            var pm = FindFirstObjectByType<PlayerMovement>();
            if (pm != null)
                player = pm.transform;
        }
    }

    void LateUpdate()
    {
        if (segments == null || segments.Length == 0) return;
        if (player == null) return;

        SortSegmentsByYDesc();

        Transform top      = segments[0];
        Transform upperMid = segments[1];
        Transform lowerMid = segments[2];
        Transform bottom   = segments[3];

        float triggerUp   = upperMid.position.y + (_height * (1f - edgePercent));
        float triggerDown = lowerMid.position.y + (_height * edgePercent);

        float playerY = player.position.y;

        // going up -> move bottom to top
        if (playerY >= triggerUp)
        {
            Vector3 p = bottom.position;
            p.y = top.position.y + _height;
            bottom.position = p;
            return;
        }

        // going down -> move top to bottom
        if (playerY <= triggerDown)
        {
            Vector3 p = top.position;
            p.y = bottom.position.y - _height;
            top.position = p;
            return;
        }
    }

    void SortSegmentsByYDesc()
    {
        for (int i = 0; i < segments.Length - 1; i++)
        {
            for (int j = i + 1; j < segments.Length; j++)
            {
                if (segments[i].position.y < segments[j].position.y)
                {
                    (segments[i], segments[j]) = (segments[j], segments[i]);
                }
            }
        }
    }

    float MeasureHeight()
    {
        if (segments != null && segments.Length > 0 && segments[0])
        {
            var r = segments[0].GetComponentInChildren<Renderer>();
            if (r != null)
                return r.bounds.size.y;
        }
        return 10f;
    }

    // ====== COMPATIBILITY WITH OLD PLAYERDRIFTCONTROLLER ======
    public void SetCurrentSpeed(float speed)
    {
        // no-op in the new system
    }
}
