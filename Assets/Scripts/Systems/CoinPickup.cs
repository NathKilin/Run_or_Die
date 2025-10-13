using UnityEngine;

/*
 * CoinPickup
 * ----------
 * PURPOSE:
 *   Lives on a coin prefab. When the player enters its trigger:
 *     1) Adds coinValue (from CollectibleSO) to PlayerStatsSO.runCoins
 *     2) Asks ScoreHUD to refresh the "COINS: X" label
 *     3) Destroys the coin GameObject (for now; later we can pool it)
 *
 * REQUIREMENTS ON THE PREFAB:
 *   - Must have a Collider2D set to isTrigger
 *
 * INSPECTOR REFERENCES:
 *   - coinDef:    the CollectibleSO asset (your Coin.asset)
 *   - stats:      the PlayerStatsSO asset (your PlayerStats.asset)
 *   - hud:        the ScoreHUD in your Canvas
 *   - playerTag:  which tag counts as the player (default "Player")
 */

[RequireComponent(typeof(Collider2D))]
public class CoinPickup : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("Collectible definition (e.g., Coin.asset with value 10).")]
    [SerializeField] private CollectibleSO coinDef;

    [Tooltip("Shared player stats (height/coins). Drag your PlayerStats.asset.")]
    [SerializeField] private PlayerStatsSO stats;

    [Header("UI")]
    [Tooltip("HUD script that shows 'COINS: X'.")]
    [SerializeField] private ScoreHUD hud;

    [Header("Player Filter")]
    [Tooltip("Only the object tagged with this will collect the coin.")]
    [SerializeField] private string playerTag = "Player";

    private void Reset()
    {
        // Ensure the collider is a trigger so the player can pass through it
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the thing entering isn't the player, ignore it
        if (!other.CompareTag(playerTag)) return;

        // Safely determine the coin value: use SO if assigned, default to 10 otherwise
        int value = (coinDef != null) ? coinDef.coinValue : 10;

        // Update the stats model
        if (stats != null)
            stats.AddCoins(value);

        // Update the HUD visual (show new total)
        if (hud != null)
            hud.SetCoins(stats != null ? stats.runCoins : 0);

        // Remove the coin (later you can pool instead of destroy)
        Destroy(gameObject);
    }
}
