using UnityEngine;

/*
 * IObstacleFactory
 * ----------------
 * Minimal interface that abstracts "how to spawn a prefab".
 * - The spawner depends on THIS interface only.
 * - Today: SimpleInstantiateFactory implements it with Instantiate(...)
 * - Tomorrow: a pooled factory can implement it and recycle objects.
 */

public interface IObstacleFactory
{
    /// <summary>
    /// Create (or fetch) an instance of 'prefab' at position/rotation.
    /// If 'parent' is given, the new object becomes its child.
    /// May return null if prefab is null.
    /// </summary>
    GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null);
}
