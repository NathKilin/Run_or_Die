using UnityEngine;

/*
 * SimpleInstantiateFactory
 * ------------------------
 * RUNTIME BEHAVIOR (very small):
 *   Implements IObstacleFactory by simply calling Unity's Instantiate.
 *
 * WHY THIS FILE:
 *   - Your spawner depends only on IObstacleFactory (an interface).
 *   - Today, we use this simple "instantiate" factory.
 *   - Tomorrow, your friend can write PooledObstacleFactory : IObstacleFactory
 *     that fetches objects from a pool WITHOUT changing the spawner code.
 *
 * HOW TO USE:
 *   - Put this component on the same GameObject as your ObstacleSpawner (or anywhere).
 *   - In ObstacleSpawner's Inspector, assign "factoryBehaviour" to this component.
 */

public class SimpleInstantiateFactory : MonoBehaviour, IObstacleFactory
{
    /// <summary>
    /// Spawns a prefab at the given position/rotation, optionally under a parent.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogWarning("SimpleInstantiateFactory.Spawn called with null prefab.");
            return null;
        }

        return Instantiate(prefab, position, rotation, parent);
    }
}
