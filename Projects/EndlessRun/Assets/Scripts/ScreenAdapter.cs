using UnityEngine;

[DisallowMultipleComponent]
public class ScreenAdapter : MonoBehaviour
{
    [Header("Scene refs")]
    public Camera targetCamera;
    public Transform topLimit, bottomLimit, leftLimit, rightLimit;
    public WorldConductor conductor;

    [Header("Margins (world units)")]
    public float verticalMargin = 0.5f;
    public float horizontalMargin = 0.5f;

    [Header("Manual overrides")]
    public bool overrideTopThreshold = false;
    public float topOverrideY = 20f;

    public bool overrideBottomThreshold = false;
    public float bottomOverrideY = 15.59f;  // <- your required value

    [Header("Transform placement")]
    [Tooltip("If true, ScreenAdapter will NOT move limit transforms; you position them manually.")]
    public bool lockLimitTransforms = true;

    void Reset() { targetCamera = Camera.main; }
    void Start() { Apply(); }
    void OnValidate() { if (!Application.isPlaying) Apply(); }

    void Apply()
    {
        if (!targetCamera) targetCamera = Camera.main;
        if (!targetCamera) return;

        var cam = targetCamera;
        float z = Mathf.Abs(cam.transform.position.z);

        // Camera-edge world points
        Vector3 top    = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, z));
        Vector3 bottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, z));
        Vector3 left   = cam.ViewportToWorldPoint(new Vector3(0f,   0.5f, z));
        Vector3 right  = cam.ViewportToWorldPoint(new Vector3(1f,   0.5f, z));

        // Move limit transforms only if not locked
        if (!lockLimitTransforms)
        {
            if (topLimit)    topLimit.position    = top;
            if (bottomLimit) bottomLimit.position = bottom;
            if (leftLimit)   leftLimit.position   = left;
            if (rightLimit)  rightLimit.position  = right;
        }

        // Compute thresholds, then apply overrides
        float topY    = top.y    + verticalMargin;
        float bottomY = bottom.y - verticalMargin;

        if (overrideTopThreshold)    topY    = topOverrideY;
        if (overrideBottomThreshold) bottomY = bottomOverrideY;

        if (conductor)
        {
            conductor.topY = topY;
            conductor.bottomY = bottomY;
        }
    }
}
