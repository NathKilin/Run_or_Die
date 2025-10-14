using System;
using Spawning;
using UnityEngine;

public class EnvironmentPiece : MonoBehaviour, IPoolable
{
    
    [HideInInspector] public MeshFilter meshFilter;
    
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null) { throw new Exception($"Environment Piece [{gameObject.name}] has no MeshFilter Component"); }
    }


    public Bounds GetWorldBounds()
    {
        return meshFilter.mesh.bounds;
    }

}
