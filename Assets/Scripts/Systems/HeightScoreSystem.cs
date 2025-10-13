using UnityEngine;

/*
 * HeightScoreSystem
 * -----------------
 *   Implements scoring rule:
 *     - Track the player's highest Y reached during THIS RUN (runMaxHeight).
 *     - Score increases ONLY when the player surpasses that previous max.
 *     - If the player falls, the score "pauses" until they climb past the max again.
 *
 *   - does NOT handle death/win/game over. It's purely scoring/visual.
 */

public class HeightScoreSystem : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Player GameObject (or any Transform that represents the player's world Y).")]
    [SerializeField] private Transform player;

    [Tooltip("The ScriptableObject that stores height and coins for this run (and best all-time).")]
    [SerializeField] private PlayerStatsSO stats;

    [Tooltip("Small UI script that prints HEIGHT and COINS.")]
    [SerializeField] private ScoreHUD hud;

    private void Start()
    {
        // Safety checks to avoid null-reference issues
        if (stats == null)
            Debug.LogWarning("HeightScoreSystem: 'stats' is not set.");
        if (hud == null)
            Debug.LogWarning("HeightScoreSystem: 'hud' is not set.");
        if (player == null)
            Debug.LogWarning("HeightScoreSystem: 'player' is not set.");

        // Reset run values at the beginning of the run
        if (stats != null)
            stats.ResetRun();

        // Initialize HUD to show "0 m" and the current coin count (usually 0 at start)
        hud?.SetHeight(0);
        if (stats != null)
            hud?.SetCoins(stats.runCoins);
    }

    private void Update()
    {
        // If we don't have the necessary refs, we do nothing safely.
        if (player == null || stats == null) return;

        // Current vertical position of the player (world space).
        float playerY = player.position.y;

        // Only update the score if we surpassed the previous max height for this run.
        if (playerY > stats.runMaxHeight)
        {
            // 1) Save the new run max
            stats.runMaxHeight = playerY;

            // 2) Update the on-screen "HEIGHT: X m"
            int meters = Mathf.FloorToInt(stats.runMaxHeight); // integer meters
            hud?.SetHeight(meters);

            // 3) keep an all-time best record  
            stats.TrySetBest(stats.runMaxHeight);
        }

        // If playerY <= runMaxHeight:
        //   Do nothing → This is EXACTLY the "score pauses on descent" behavior you wanted.
    }
}
