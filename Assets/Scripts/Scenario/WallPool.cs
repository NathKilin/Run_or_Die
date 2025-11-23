using UnityEngine;

public class WallPool : MonoBehaviour
{
    // each piece pf wall called "segment
    [Header("Segments (exactly 4, ordered doesn't matter)")]
    public Transform[] segments;

    [Header("Player to track")]
    [SerializeField] private Transform player;

    [Header("Segment size")]
    [Tooltip("If 0, we will measure the height from the first segment renderer.")]
    // Height of ONE wall segment.
    // If hight = 0 we try to read the Renderer bounds to know the height.
    public float segmentHeight = 0f;

    [Header("How close to the edge before we swap")]
    [Range(0.05f, 0.5f)]
    // This is a percentage of the segment to be our "trigger zone".
    // 0.2f = 20% from the top / bottom of the *middle pair* (the ones player should stay between).
    public float edgePercent = 0.2f;

    // cached real height we ended up using
    private float _height;

    void Start()
    {
        // 1) Decide how tall a segment is
        //    If you set it in the Inspector, we use that 👇🏻
        //    If not, we try to measure from the mesh/renderer.
        _height = (segmentHeight > 0f) ? segmentHeight : MeasureHeight();

        // 2) If no player was dragged in the Inspector, try to auto-find one
        if (player == null)
        {
            var pm = FindFirstObjectByType<PlayerMovement>();
            if (pm != null)
                player = pm.transform;
        }
    }

    void LateUpdate()
    {
        // If something is missing, do nothing
        if (segments == null || segments.Length == 0) return;
        if (player == null) return;

        // Every frame we sort the 4 segments by their Y (from highest to lowest).
        SortSegmentsByYDesc();

        Transform top      = segments[0]; 
        Transform upperMid = segments[1]; 
        Transform lowerMid = segments[2]; 
        Transform bottom   = segments[3]; 

        // We want the player to stay BETWEEN upperMid (B) and lowerMid (C).
        // So we create two trigger lines:
        // going UP: if player goes too high inside B, we need to bring the BOTTOM segment to the TOP
        // going DOWN: if player goes too low inside C, we need to bring the TOP segment to the BOTTOM

        // "top trigger" for going UP:
        //   go UP almost a full segment, but keep a small margin (1 - edgePercent)
        float triggerUp   = upperMid.position.y + (_height * (1f - edgePercent));

        // "bottom trigger" for going DOWN:
        //   start from lowerMid.y (the segment C bottom)
        //   go slightly UP from there (edgePercent of the height)
        float triggerDown = lowerMid.position.y + (_height * edgePercent);

        // current vertical position of the player
        float playerY = player.position.y;

        //PLAYER GOING UP
        if (playerY >= triggerUp)
        {
            // copy bottom's position
            Vector3 p = bottom.position;

            // place it above the current top segment by exactly 1 segment height
            p.y = top.position.y + _height;

            // apply
            bottom.position = p;

            // we can return because we already fixed the layout this frame
            return;
        }

        //PLAYER GOING DOWN 
        if (playerY <= triggerDown)
        {
            Vector3 p = top.position;

            // put it right below the current bottom segment
            p.y = bottom.position.y - _height;

            top.position = p;
            return;
        }

        // If neither case triggers, we do nothing this frame — the 4 walls just stay.
    }

    // Small helper: sorts the "segments" array by Y (descending)
    void SortSegmentsByYDesc()
    {
        // super simple bubble sort, good enough for 4 items
        for (int i = 0; i < segments.Length - 1; i++)
        {
            for (int j = i + 1; j < segments.Length; j++)
            {
                // if the one at i is LOWER than the one at j, swap them
                if (segments[i].position.y < segments[j].position.y)
                {
                    (segments[i], segments[j]) = (segments[j], segments[i]);
                }
            }
        }
    }

    // Tries to measure the height of 1 segment automatically
    float MeasureHeight()
    {
        // do we have at least one segment to look at?
        if (segments != null && segments.Length > 0 && segments[0])
        {
            // get any renderer (mesh, sprite, etc.)
            var r = segments[0].GetComponentInChildren<Renderer>();
            if (r != null)
                // size.y = height in world units
                return r.bounds.size.y;
        }

        // fallback value — use something reasonable for your scene
        return 10f;
    }
    public void SetCurrentSpeed(float speed)
    {
        // no-op in the new system
    }
}
