using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // direct reference to the player (drag in Inspector)
    [SerializeField] private Transform target;

    // how fast the camera follows the player
    [SerializeField] private float followSpeed = 5f;

    // camera doesn't move in X or Z
    private float fixedX;
    private float fixedZ;

    void Start()
    {
        // remember where the camera started
        fixedX = transform.position.x;
        fixedZ = transform.position.z;

        // if no target was set, try to find one
        if (target == null)
        {
            var player = FindFirstObjectByType<PlayerMovement>(); // or your player script
            if (player != null)
                target = player.transform;
        }
    }

    void LateUpdate()
    {
        // if we still don't have a target, do nothing
        if (target == null) return;

        Vector3 currentPos = transform.position;

        // target Y is player's Y
        float targetY = target.position.y;

        // make it smooth
        float newY = Mathf.Lerp(currentPos.y, targetY, followSpeed * Time.deltaTime);

        // X and Z stay static, only Y moves
        transform.position = new Vector3(fixedX, newY, fixedZ);
    }
}
