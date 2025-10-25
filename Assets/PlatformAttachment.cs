using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformAttachment : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject platformPrefab;

    [Header("Count")]
    [SerializeField, Range(0, 8)] private int maxPlatforms = 4;

    [Header("Sizing")]
    [SerializeField, Min(1)] private int minWidthX = 1;
    [SerializeField, Min(1)] private int maxWidthX = 5;
    [SerializeField] private float heightY = 1f;
    [SerializeField] private float depthZ  = 2f;

    [Header("Grid & Band")]
    [SerializeField] private int   minGridX = -1;
    [SerializeField] private int   maxGridX =  4;
    [SerializeField] private float wallHeight = 30f;
    [SerializeField, Range(0f,1f)] private float safeMin01 = 0.10f;
    [SerializeField, Range(0f,1f)] private float safeMax01 = 0.90f;

    [Header("Z Placement")]
    [SerializeField] private float zAt = 2f;
    [SerializeField] private float zNudgePerIndex = 0.02f;

    [Header("Separation")]
    [SerializeField, Min(0)] private int gapCells = 1;

    [Header("Lifecycle")]
    [SerializeField] private bool spawnOnEnable = true;

    // ——— INTERNAL STATE ———
    private readonly List<GameObject> _spawned = new();
    private Transform _platformsRoot; // child named "Platforms" on this wall

    void Awake()
    {
        _platformsRoot = transform.Find("Platforms");
        if (_platformsRoot == null)
        {
            _platformsRoot = new GameObject("Platforms").transform;
            _platformsRoot.SetParent(transform, false);
        }
    }

    void OnEnable()
    {
        if (spawnOnEnable) SpawnAll();
    }

    void OnDisable()
    {
        ClearAll(); // destroys only what THIS instance spawned
    }

    public void SpawnAll()
    {
        ClearAll();
        if (!platformPrefab) return;

        int cols = maxGridX - minGridX + 1;
        if (cols <= 0) return;

        bool[] occ = new bool[cols];
        int targetCount = Random.Range(0, maxPlatforms + 1);

        int placed = 0;
        int safety = 40;

        Debug.Log($"[PlatformAttachment:{name}] SpawnAll target={targetCount}");

        while (placed < targetCount && safety-- > 0)
        {
            int width = Random.Range(minWidthX, maxWidthX + 1);

            // gather candidates that fit with gap
            List<int> candidates = new List<int>();
            for (int gx = minGridX; gx <= maxGridX; gx++)
            {
                int s = gx - minGridX;
                int e = s + width - 1;
                if (e < 0 || e >= cols) continue;

                int pS = Mathf.Max(0, s - gapCells);
                int pE = Mathf.Min(cols - 1, e + gapCells);

                bool blocked = false;
                for (int i = pS; i <= pE; i++)
                    if (occ[i]) { blocked = true; break; }

                if (!blocked) candidates.Add(gx);
            }

            if (candidates.Count == 0) break;

            int chosenGX = candidates[Random.Range(0, candidates.Count)];

            // reserve cells + gap
            int rs = chosenGX - minGridX;
            int re = rs + width - 1;
            int rS = Mathf.Max(0, rs - gapCells);
            int rE = Mathf.Min(cols - 1, re + gapCells);
            for (int i = rS; i <= rE; i++) occ[i] = true;

            // position
            float y01 = Random.Range(Mathf.Min(safeMin01, safeMax01), Mathf.Max(safeMin01, safeMax01));
            float y   = wallHeight * y01;
            float z   = zAt + placed * zNudgePerIndex;

            Vector3 worldPos = new Vector3(transform.position.x + chosenGX,
                                           transform.position.y + y,
                                           z);

            var go = Instantiate(platformPrefab, worldPos, Quaternion.identity);
            go.name = $"{platformPrefab.name}_Platform_{placed+1}";
            go.transform.SetParent(_platformsRoot, true); // << isolate under Platforms
            go.transform.localScale = new Vector3(width, heightY, depthZ);

            var col = go.GetComponent<Collider>();
            if (!col) col = go.AddComponent<BoxCollider>();
            col.isTrigger = false;

            _spawned.Add(go);

            Debug.Log($"[PlatformAttachment:{name}] SPAWN idx={placed} x={chosenGX} w={width} y={y:F2} z={z:F2} go={go.GetInstanceID()}");
            placed++;
        }
    }

    public void ClearAll()
    {
        // Destroy only what this instance spawned
        for (int i = _spawned.Count - 1; i >= 0; i--)
        {
            var go = _spawned[i];
            if (go != null)
            {
                Debug.Log($"[PlatformAttachment:{name}] DESTROY go={go.GetInstanceID()}");
                Destroy(go);
            }
        }
        _spawned.Clear();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        float y0 = wallHeight * Mathf.Min(safeMin01, safeMax01);
        float y1 = wallHeight * Mathf.Max(safeMin01, safeMax01);

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.2f);
        Vector3 c = new Vector3(transform.position.x + (minGridX + maxGridX) * 0.5f,
                                transform.position.y + (y0 + y1) * 0.5f,
                                zAt);
        Vector3 s = new Vector3((maxGridX - minGridX + 1) + 0.25f,
                                Mathf.Abs(y1 - y0),
                                0.2f);
        Gizmos.DrawCube(c, s);
    }
#endif
}
