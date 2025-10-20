using System.Collections.Generic;
using UnityEngine;

/// Scrolls & recycles wall segments based on player's vertical motion.
/// Uses player's Rigidbody velocity when non-kinematic; otherwise uses PlayerMover.VerticalSpeed.
[DisallowMultipleComponent]
public class WorldConductor : MonoBehaviour
{
    [Header("Scene Refs")]
    public PlayerMover player;
    public List<Transform> segments = new List<Transform>();

    [Header("Scroll Tuning")]
    [Tooltip("worldSpeed = -playerVy * speedScale (paused in dead-zone/resting)")]
    public float speedScale = 1.0f;

    [Tooltip("Vertical gap between consecutive segments when recycling")]
    public float segmentGapY = 10f;

    [Header("Recycle Thresholds (auto-set by ScreenAdapter)")]
    public float topY = 20f;
    public float bottomY = -20f;

    Rigidbody _playerRb;

    void Awake()
    {
        if (player) _playerRb = player.GetComponent<Rigidbody>();
    }

    float HighestY()
    {
        float y = float.NegativeInfinity;
        for (int i = 0; i < segments.Count; i++)
            if (segments[i] && segments[i].position.y > y) y = segments[i].position.y;
        return y;
    }

    float LowestY()
    {
        float y = float.PositiveInfinity;
        for (int i = 0; i < segments.Count; i++)
            if (segments[i] && segments[i].position.y < y) y = segments[i].position.y;
        return y;
    }

    void LateUpdate()
    {
        if (!player) return;

        // Read actual vertical speed
        float playerVy = 0f;
        if (_playerRb != null && !_playerRb.isKinematic)
            playerVy = _playerRb.linearVelocity.y;
        else
            playerVy = player.VerticalSpeed;

        // Dead-zone near apex or if player reports resting
        const float eps = 0.05f;
        float worldSpeed;
        if (player.IsResting || Mathf.Abs(playerVy) < eps)
            worldSpeed = 0f;
        else
            worldSpeed = -playerVy * speedScale;

        // Move all segments
        Vector3 delta = Vector3.up * worldSpeed * Time.deltaTime;
        for (int i = 0; i < segments.Count; i++)
            if (segments[i]) segments[i].position += delta;

        // Recycle depending on direction
        if (worldSpeed < 0f) // world moving down
        {
            for (int i = 0; i < segments.Count; i++)
            {
                var t = segments[i]; if (!t) continue;
                if (t.position.y < bottomY)
                {
                    float topAfter = HighestY();
                    t.position = new Vector3(t.position.x, topAfter + segmentGapY, t.position.z);
                    t.GetComponent<WallSegmentSpawner>()?.RespawnContents();
                    t.GetComponent<WallBladeSpawner>()?.RespawnContents();
                }
            }
        }
        else if (worldSpeed > 0f) // world moving up
        {
            for (int i = 0; i < segments.Count; i++)
            {
                var t = segments[i]; if (!t) continue;
                if (t.position.y > topY)
                {
                    float bottomAfter = LowestY();
                    t.position = new Vector3(t.position.x, bottomAfter - segmentGapY, t.position.z);
                    t.GetComponent<WallSegmentSpawner>()?.RespawnContents();
                    t.GetComponent<WallBladeSpawner>()?.RespawnContents();
                }
            }
        }
    }
}
