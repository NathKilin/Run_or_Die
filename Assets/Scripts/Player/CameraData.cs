using System;
using UnityEngine;

public class CameraData : MonoBehaviour
{

    public static Camera camera;


    private void Awake()
    {
        camera = GetComponent<Camera>();
    }


    public static bool IsObjectBelowViewport(GameObject obj)
    {
        // TODO 
        // Add additional calculations whether an obj has a mesh so the top part of it is gone into account
        
        // MeshFilter mf = obj.GetComponent<MeshFilter>();
        // if (mf != null) {}
        
        return GetObjectRelativeHeight(obj) < 0;
    }
    
    
    public static float GetObjectRelativeHeight(GameObject obj)
    {
        return camera.WorldToViewportPoint(obj.transform.position).y;
    }
}
