using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    PlayerMovement player;

    [SerializeField, Range(0f,1f)] private float heightToFollowUp = .65f; 
    [SerializeField, Range(0f,1f)] private float heightToFollowDown = .35f;

    [SerializeField] private float cameraSpeed = 20f;
    

    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
    }
    
    
    void Update()
    {
        float playerHeight = CameraData.GetObjectRelativeHeight(player.gameObject);
        if (playerHeight > heightToFollowUp || playerHeight < heightToFollowDown) {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * cameraSpeed),
                transform.position.z
                );
        }
    }
}
