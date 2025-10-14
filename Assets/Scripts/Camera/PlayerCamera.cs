using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CameraData cameraData;
    private Camera camera;
    public GameObject player;
    
    [Header("Camera Follow Values")]
    public float screenRelativeHeightToFollowPlayer = .6f;
    public float lerpSpeedFollow = .5f;
    
    
    void Start()
    {
        camera = GetComponent<Camera>();
        cameraData = GetComponent<CameraData>();    
    }

    
    void Update()
    {
        float relativeHeight = cameraData.GetPlayerRelativeHeightToCamera();
        if (relativeHeight >= screenRelativeHeightToFollowPlayer
            || relativeHeight <= 1f - screenRelativeHeightToFollowPlayer)
        {
            Vector3 newPosition = new Vector3(
                transform.position.x,
                player.transform.position.y,
                transform.position.z);
            camera.transform.position = Vector3.Lerp(
                camera.transform.position,
                newPosition,
                lerpSpeedFollow * Time.deltaTime);
        }    
    }
}
