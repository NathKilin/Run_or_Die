using System;
using UnityEngine;

public class CameraData : MonoBehaviour
{
    public GameObject player;

    [HideInInspector] public static Camera camera;


    void Start()
    {
        camera = GetComponent<Camera>();
    }


    public float GetPlayerRelativeHeightToCamera()
    {
        return Mathf.Clamp01(
            camera.WorldToViewportPoint(
                player.transform.position
                ).y
        );
    }
    
    
    public static bool IsObjectInCameraBounds(GameObject obj)
    {
        float distanceToObject = obj.transform.position.z - camera.transform.position.z;
        return obj.transform.position.y > GetCameraBottom(distanceToObject).y && obj.transform.position.y < GetCameraTop(distanceToObject).y;
    }
    
    
    public static Vector3 GetCameraTop(float distance)
    {
        return camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, distance));
    }

    
    public static Vector3 GetCameraBottom(float distance)
    {
        return camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, distance));
    }
    
}
