using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WallBladeSpawner : MonoBehaviour
{
    [Header("Prefab (assign your Blade prefab)")]
    public GameObject bladePrefab;

    [Header("Local-space placement")]
    public float xMin = 0.5f, xMax = 7.75f;
    public float yMin = -4.5f, yMax = 4.5f;
    public float zLocal = 0.5f;

    [Header("Counts & spacing")]
    [Range(0, 3)] public int maxBlades = 3;
    public float minDistance = 1.25f;

    [Header("Orientation")]
    public float xRotation = 90f;

    void Start() => RespawnContents();

    public void RespawnContents()
    {
        if (!bladePrefab)
        {
            Debug.LogWarning($"{name}: bladePrefab not assigned.");
            return;
        }
        if (bladePrefab.GetComponent<WallBladeSpawner>() != null)
        {
            Debug.LogError($"{name}: Blade prefab MUST NOT have WallBladeSpawner on it.");
            return;
        }

        // Clear old
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var c = transform.GetChild(i);
            if (c && c.name.StartsWith("Blade_"))
                Destroy(c.gameObject);
        }

        // Decide count 0..maxBlades
        int target = Random.Range(0, maxBlades + 1);

        // Place with spacing
        var placed = new List<Vector3>();
        int safety = 0;

        while (placed.Count < target && safety++ < 100)
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);
            var candidate = new Vector3(x, y, zLocal);

            bool ok = true;
            for (int i = 0; i < placed.Count; i++)
                if (Vector3.Distance(candidate, placed[i]) < minDistance) { ok = false; break; }

            if (!ok) continue;

            var b = Instantiate(bladePrefab, transform);
            b.name = $"Blade_{placed.Count + 1}";
            b.transform.localPosition = candidate;
            b.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);

            placed.Add(candidate);
        }
    }
}
