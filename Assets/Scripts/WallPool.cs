using UnityEngine;

public class WallPool : MonoBehaviour
{
    [Header("Segments (drag the 4 segments here)")]
    public Transform[] segments;

    [Header("Motion")]
    [Tooltip("Base climb speed; Player may override at runtime with positive/negative values.")]
    public float speed = 5f;           // default magnitude
    public float despawnY = -20f;      // Y position to recycle segments

    [Header("Height (0 = auto from Renderer)")]
    public float segmentHeight = 0f;   // if 0, auto-calc from Renderer

    float _height;

    // === NEW: externally controlled, signed speed ===
    public float CurrentSpeed { get; private set; }

    void Start()
    {
        _height = (segmentHeight > 0f) ? segmentHeight : MeasureHeight();
        CurrentSpeed = speed; // default: keep previous behavior
    }

    void Update()
    {
        if (segments == null || segments.Length == 0) return;

        // Move world by signed CurrentSpeed
        Vector3 delta = Vector3.down * CurrentSpeed * Time.deltaTime;
        for (int i = 0; i < segments.Length; i++)
            segments[i].position += delta;

        // Recycle segments that passed despawnY
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].position.y < despawnY)
            {
                float highestY = GetHighestY();
                Vector3 pos = segments[i].position;
                pos.y = highestY + _height;
                segments[i].position = pos;
            }
        }
    }

    // === NEW: public setter ===
    public void SetCurrentSpeed(float s)
    {
        CurrentSpeed = s;
    }

    float GetHighestY()
    {
        float highest = float.NegativeInfinity;
        if (segments != null)
        {
            for (int i = 0; i < segments.Length; i++)
                if (segments[i] && segments[i].position.y > highest)
                    highest = segments[i].position.y;
        }
        return (highest == float.NegativeInfinity) ? 0f : highest;
    }

    float MeasureHeight()
    {
        if (segments != null && segments.Length > 0 && segments[0])
        {
            var r = segments[0].GetComponentInChildren<Renderer>();
            if (r != null) return r.bounds.size.y;
        }
        return 10f; // fallback
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-100f, despawnY, 0f), new Vector3(100f, despawnY, 0f));
    }
#endif
}
