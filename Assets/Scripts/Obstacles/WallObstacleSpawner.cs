using UnityEngine;
using System.Collections.Generic;

// MonoBehaviour = a Unity class that allows this script to be attached to GameObjects
// It gives access to things like Start(), Update(), Transform, OnEnable() etc.
public class WallObstacleSpawner : MonoBehaviour
{
    [Header("Blade Settings")]
    public GameObject bladePrefab;
    [Range(0, 4)] public int maxBlades = 4;

    // X and Z spawn rules
    readonly int[] possibleX = { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6 };
    const float bladeZ = 2f;

    // Wall stats
    const float wallHeight = 30f;
    const float safePercent = 0.4f; // middle 80% of the wall

    List<GameObject> spawnedBlades = new List<GameObject>();

    // OnEnable() is called every time the object becomes active
    void OnEnable()
    {
        SpawnRandomBlades();
    }

    // OnDisable() is called when object is deactivated (or recycled in pooling)
    void OnDisable()
    {
        ClearBlades();
    }

    void SpawnRandomBlades()
    {
        ClearBlades();

        int count = Random.Range(0, maxBlades + 1);

        // Y range logic (Safe Center Area)
        float safeYRange = wallHeight * safePercent;
        float minY = transform.position.y - safeYRange;
        float maxY = transform.position.y + safeYRange;

        for (int i = 0; i < count; i++)
        {
            int x = possibleX[Random.Range(0, possibleX.Length)];
            float y = Random.Range(minY, maxY);

            Vector3 pos = new Vector3(x, y, bladeZ);

            // Quaternion.identity = x rotation (90, 0, 0)
            Quaternion rot = Quaternion.Euler(90f, 0f, 0f);
            // "transform" = makes the blade a child of this wall (so it moves with the wall)
            GameObject blade = Instantiate(bladePrefab, pos, rot, transform);

            spawnedBlades.Add(blade);
        }
    }

    void ClearBlades()
    {
        for (int i = 0; i < spawnedBlades.Count; i++)
            Destroy(spawnedBlades[i]);

        spawnedBlades.Clear();
    }
}
