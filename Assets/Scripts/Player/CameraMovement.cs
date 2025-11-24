using UnityEngine;
using System;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float followSpeed = 5f;

    [Header("Fall Detection")]
    [SerializeField] private float fallThreshold = 10f;      // how much height per frame is too much - player falls too much

    private float fixedX;
    private float fixedZ;




    void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;

        if (target == null)
        {
            var player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
                target = player.transform;
        }
        
    }


    void Update()
    {
        if (target == null) return;
        
        FollowPlayer();

        CheckPlayerBounds();
    }


    private void FollowPlayer()
    {
        Vector3 currentPos = transform.position;
        float targetY = target.position.y;

        if (targetY < currentPos.y) {
            return;
            // Don't go down
        }

        float newY = Mathf.Lerp(currentPos.y, targetY, followSpeed * Time.deltaTime);
        transform.position = new Vector3(fixedX, newY, fixedZ);
    }


    private void CheckPlayerBounds()
    {
        // check if player is in bounds
        if (CameraData.GetObjectRelativeHeight(target.gameObject) < -.1f)
            // if not restart
            GameManager.Instance.GameOver();
    }
    

}
