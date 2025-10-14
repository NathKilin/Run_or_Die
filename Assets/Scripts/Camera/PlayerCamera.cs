using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CameraData cameraData;

    [Header("Camera Follow Values")]
    public float screenRelativeHeightToFollowPlayer = .6f;
    public float lerpSpeedFollow = .5f;
    
    
    void Start()
    {
        cameraData = GetComponent<CameraData>();    
    }

    
    void Update()
    {
        if (cameraData.GetPlayerRelativeHeightToCamera() >= screenRelativeHeightToFollowPlayer)
        {
            
        }    
    }
}
