using System;
using System.Collections.Generic;
using UnityEngine;
using Spawning;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour, IObjectPooler
{
    
    private List<GameObject> objectPool;
    private List<GameObject> spawnedObjects;
    public GameObject[] environmentPrefabs;
    public int countPerObject = 7;

    public GameObject player;
    
    
    public void Start()
    {
        InitializePool();
        GenerateInitialEnvironment();
    }


    private void GenerateInitialEnvironment()
    {
        for (int i = 0; i < countPerObject; i++) {
            SpawnNewEnvironmentPiece();
        }
    }


    private void SpawnNewEnvironmentPiece()
    {
        // 1. Get Random Piece
        bool didGetObject = false;
        GameObject obj = null;
        while (!didGetObject) {
            obj = objectPool[Random.Range(0, objectPool.Count)];
            if (obj != null) { didGetObject = true; }
        }
        
        // 2. Spawn Piece
        Vector3 positionToSpawn = Vector3.zero;

        //      2.1. Get Piece Size
        obj.SetActive(true); // Temporarily Enable EnvPiece To Calculate Bounds 
        EnvironmentPiece environmentPiece = obj.GetComponent<EnvironmentPiece>();
        Bounds pieceBounds = environmentPiece.GetWorldBounds();
        obj.SetActive(false);
        
        //      2.2. Figure out where to spawn piece
        bool isOtherObject = spawnedObjects.Count != 0;
        if (isOtherObject) {
            obj.SetActive(true);
            EnvironmentPiece topPiece = spawnedObjects[spawnedObjects.Count - 1].GetComponent<EnvironmentPiece>(); 
            Bounds topPieceBounds = topPiece.GetWorldBounds();
            obj.SetActive(false);
            positionToSpawn = topPiece.transform.position;
            positionToSpawn.z = topPieceBounds.max.z + pieceBounds.extents.z;
        } else {
            float zDistanceToPlayer = player.transform.position.z - CameraData.camera.transform.position.z;
            positionToSpawn = CameraData.GetCameraBottom(zDistanceToPlayer);
            positionToSpawn.z += pieceBounds.extents.z;
        }
        
        //      2.3. Spawn Piece At Position
        ActivateObject(obj,positionToSpawn);

    }
    
    
    public void InitializePool()
    {
        objectPool = new List<GameObject>();
        spawnedObjects = new List<GameObject>();
        foreach (GameObject obj in environmentPrefabs) {
            if (obj.GetComponent<EnvironmentPiece>() == null) { throw new Exception($"Object [{obj.name}] added to environment prefabs, is not an environment piece."); }
            
            for (int i = 0; i < countPerObject; i++) {
                GameObject newObject = Instantiate(obj);
                objectPool.Add(newObject);
                newObject.SetActive(false);
            }
        }
    }

    
    public void ActivateObject(GameObject obj, Vector3 position)
    {
        objectPool.Remove(obj);
        spawnedObjects.Add(obj);
        obj.SetActive(true);
        obj.transform.position = position;
    }

    
    public void ReturnObject(GameObject obj)
    {
        spawnedObjects.Remove(obj);
        objectPool.Add(obj);
        obj.SetActive(false);
    }
}
