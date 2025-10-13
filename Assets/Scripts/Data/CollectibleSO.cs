using UnityEngine;

/*
 * CollectibleSO
 * -------------
 * DATA ONLY for a collectible type (just the coin for now)
 * to create an asset and set its values.
 *
 * For now:
 *   - id: short identifier (e.g., "coin")
 *   - coinValue: how many coins this collectible gives
 *
 */

[CreateAssetMenu(menuName = "RunOrDie/Collectible")]
public class CollectibleSO : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Short ID (e.g., 'coin'). Useful if you expand later.")]
    public string id = "coin";

    [Header("Coin Settings")]
    [Tooltip("How many coins this collectible grants.")]
    public int coinValue = 10;
}
