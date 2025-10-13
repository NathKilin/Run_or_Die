using UnityEngine;
using TMPro; // we can swap to TMP_Text if we want it later

/*
 * ScoreHUD
 * --------
 * Minimal UI bridge that displays:
 *   - HEIGHT: <meters> m
 *   - COINS: <count>
 *
 * It only shows values given by other systems.
 */

public class ScoreHUD : MonoBehaviour
{
    [Header("UI References (drag from your Canvas)")]
    [Tooltip("TMP text that will show the current height, e.g., 'HEIGHT: 42 m'.")]
    [SerializeField] private TMP_Text heightText;

    [Tooltip("TMP text that will show the coin total, e.g., 'COINS: 30'.")]
    [SerializeField] private TMP_Text coinsText;

    // Update the height label in meters (integer for a clean look).
    // Call this whenever the player's max height increases.
    public void SetHeight(int meters)
    {
        if (!heightText) return; // safe guard in case not assigned
        heightText.text = $"HEIGHT: {meters} m";
    }

    /// Update the coins label.
    /// Call this whenever the player picks up a coin.
    public void SetCoins(int coins)
    {
        if (!coinsText) return;
        coinsText.text = $"COINS: {coins}";
    }
}
