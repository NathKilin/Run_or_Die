using UnityEngine;

public class WallPool : MonoBehaviour
{
    [Header("Segments (drag the 4 segments here)")]
    public Transform[] segments;

    [Header("Motion")]
    public float speed = 5f;           // units per second
    public float despawnY = -20f;      // Y position to recycle segments

    [Header("Height (0 = auto from Renderer)")]
    public float segmentHeight = 0f;   // if 0, auto-calculate from Renderer

    float _height;

    void Start()
    {
        // Decide the height to jump upward when recycling
        _height = (segmentHeight > 0f) ? segmentHeight : MeasureHeight();
    }

    void Update()
    {
        if (segments == null || segments.Length == 0) return;

        // 1) Move every wall down
        Vector3 delta = Vector3.down * speed * Time.deltaTime;
        for (int i = 0; i < segments.Length; i++)
            segments[i].position += delta;

        // 2) Recycle any that crossed the despawnY line
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].position.y < despawnY)
            {
                float highestY = GetHighestY();
                Vector3 pos = segments[i].position;
                pos.y = highestY + _height;       // jump above the top by exactly one height
                segments[i].position = pos;
            }
        }
    }

    float GetHighestY()
    {
        float h = float.NegativeInfinity;
        for (int i = 0; i < segments.Length; i++)
            if (segments[i].position.y > h) h = segments[i].position.y;
        return h;
    }

    float MeasureHeight()
    {
        // Try to read vertical size from the first segment's Renderer bounds
        if (segments != null && segments.Length > 0)
        {
            var r = segments[0].GetComponentInChildren<Renderer>();
            if (r != null) return r.bounds.size.y;
        }
        return 10f; // fallback if no renderer found; adjust as needed
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draw the despawn line to make logic visible
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-100f, despawnY, 0f), new Vector3(100f, despawnY, 0f));
    }
#endif
}
