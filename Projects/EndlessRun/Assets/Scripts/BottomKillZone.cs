using UnityEngine;

public class BottomKillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Detect player by component (safer than tag)
        var mover = other.GetComponent<PlayerMover>();
        if (mover == null) return;

        // Freeze the world
        mover.enabled = false;                          // stop player input/movement
        var conductor = FindObjectOfType<WorldConductor>();
        if (conductor) conductor.enabled = false;       // stop world scrolling

        Time.timeScale = 0f;                            // hard freeze
        Debug.Log("Game Over: fell below bottom.");
        // TODO: show UI / restart
    }
}
