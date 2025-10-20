using System.Collections.Generic;
using UnityEngine;

public class WallSegmentSpawner : MonoBehaviour
{
    [Header("Prefab (assign your Platform prefab)")]
    public GameObject platformPrefab;

    [Header("Local-space placement")]
    public float xMin = -2.65f, xMax = 2.40f;
    public float yMin = -4.5f, yMax = 4.5f;
    public float zLocal = 5.46f;  // use YOUR value

    [Header("Counts & spacing")]
    [Range(0,3)] public int maxPlatforms = 3;
    public float minYGap = 0.6f;

    void Start() => RespawnContents();   // spawn at Play
    public void RespawnContents()
    {
        if (!platformPrefab)
        {
            Debug.LogWarning($"{name}: platformPrefab not assigned.");
            return;
        }

        // Clear old
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var c = transform.GetChild(i);
            if (c && c.name.StartsWith("Platform")) Destroy(c.gameObject);
        }

        int target = Random.Range(0, maxPlatforms + 1);
        var ys = new List<float>();
        int safety = 0;

        while (ys.Count < target && safety++ < 50)
        {
            float y = Random.Range(yMin, yMax);
            bool ok = true;
            for (int i = 0; i < ys.Count; i++)
                if (Mathf.Abs(y - ys[i]) < minYGap) { ok = false; break; }

            if (!ok) continue;
            ys.Add(y);

            float x = Random.Range(xMin, xMax);
            var p = Instantiate(platformPrefab, transform);
            p.name = "Platform_" + ys.Count;
            p.transform.localPosition = new Vector3(x, y, zLocal);
        }
    }
}
