using UnityEngine;

public class OrientationScaler : MonoBehaviour
{
    public enum PreviewMode
    {
        Auto,           // use real screen aspect
        ForcePortrait,  // pretend it's portrait
        ForceLandscape  // pretend it's landscape
    }

    [Header("Preview / Debug")]
    public PreviewMode previewMode = PreviewMode.Auto;
    public bool livePreview = true;   // re-apply every frame while tuning

    [Header("References")]
    public Transform wallsRoot;       // parent of the 4 wall segments
    public PlayerDriftController driftPlayer;  // optional, if you ever add it
    public PlayerMovement runnerPlayer;        // optional

    [Header("Portrait setup")]
    public float portraitWallScaleX = 1.0f;

    [Header("Landscape setup")]
    public float landscapeWallScaleX = 1.25f;

    // cache
    private bool _isPortrait;

    void Start()
    {
        // try to auto-find players if you forgot
        if (driftPlayer == null)
            driftPlayer = FindFirstObjectByType<PlayerDriftController>();
        if (runnerPlayer == null)
            runnerPlayer = FindFirstObjectByType<PlayerMovement>();

        ApplyByAspect();
    }

    void Update()
    {
        if (livePreview)
        {
            ApplyByAspect();   // so changing values in Inspector shows right away
        }
        else
        {
            bool nowPortrait = ComputePortraitFlag();
            if (nowPortrait != _isPortrait)
                ApplyByAspect();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            ApplyByAspect();
        }
    }
#endif

    // -------------------------------------------------- helpers

    bool ComputePortraitFlag()
    {
        switch (previewMode)
        {
            case PreviewMode.ForcePortrait:  return true;
            case PreviewMode.ForceLandscape: return false;
            case PreviewMode.Auto:
            default:
                return Screen.height >= Screen.width;
        }
    }

    void ApplyByAspect()
    {
        _isPortrait = ComputePortraitFlag();

        if (_isPortrait)
            ApplyPortrait();
        else
            ApplyLandscape();
    }

    void ApplyPortrait()
    {
        if (wallsRoot != null)
        {
            Vector3 s = wallsRoot.localScale;
            s.x = portraitWallScaleX;
            wallsRoot.localScale = s;
        }
    }

    void ApplyLandscape()
    {
        if (wallsRoot != null)
        {
            Vector3 s = wallsRoot.localScale;
            s.x = landscapeWallScaleX;
            wallsRoot.localScale = s;
        }
    }
}
