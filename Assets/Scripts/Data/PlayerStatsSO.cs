using UnityEngine;

/*
 * PlayerStatsSO
 * -------------
 * A simple data container (ScriptableObject) for the player's scoring info.
 * It keeps track of:
 *   - bestHeightMeters: your all-time best record 
 *   - runMaxHeight: highest Y reached in the current run (resets each run)
 *   - runCoins: coins collected in the current run (resets each run)
 */

[CreateAssetMenu(menuName = "RunOrDie/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Lifetime (change only at runtime or via code)")]
    [Tooltip("All-time best height in meters (float).")]
    public float bestHeightMeters = 0f;

    [Header("Current Run (reset every time a new run starts)")]
    [System.NonSerialized] public float runMaxHeight = 0f; // highest Y reached this run
    [System.NonSerialized] public int runCoins = 0;        // coins collected this run

    // Call this at the start of a run (e.g., when the level/scene loads).
    public void ResetRun()
    {
        runMaxHeight = 0f;
        runCoins = 0;
    }

    // Increase the coin count for this run.
    public void AddCoins(int amount)
    {
        runCoins += Mathf.Max(0, amount); // prevent negative adds
    }

    // If 'height' beats the all-time best, update it.
    public void TrySetBest(float height)
    {
        if (height > bestHeightMeters)
            bestHeightMeters = height;
    }
}
