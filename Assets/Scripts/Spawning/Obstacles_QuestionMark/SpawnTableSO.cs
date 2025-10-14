using UnityEngine;

/*
 * SpawnTableSO
 * ------------
 * DATA ONLY: Holds a list of ObstacleSpawnRuleSO entries.
 * Given a height (where you plan to spawn), Pick(atHeight) chooses ONE rule
 * using weighted random among the rules eligible for that height.
 */

[CreateAssetMenu(menuName = "RunOrDie/Spawning/Spawn Table")]
public class SpawnTableSO : ScriptableObject
{
    [Tooltip("List of possible obstacle rules to pick from.")]
    public ObstacleSpawnRuleSO[] rules;

    /// <summary>
    /// Choose a rule by weighted random among those whose height range
    /// contains 'atHeight'. Returns null if nothing is eligible.
    /// </summary>
    public ObstacleSpawnRuleSO Pick(float atHeight)
    {
        if (rules == null || rules.Length == 0) return null;

        // 1) Sum weights of eligible rules
        float total = 0f;
        for (int i = 0; i < rules.Length; i++)
        {
            var r = rules[i];
            if (r == null) continue;
            if (atHeight < r.minHeight || atHeight > r.maxHeight) continue;

            total += Mathf.Max(0f, r.weight);
        }
        if (total <= 0f) return null;

        // 2) Single roll and walk the list
        float roll = Random.value * total;
        for (int i = 0; i < rules.Length; i++)
        {
            var r = rules[i];
            if (r == null) continue;
            if (atHeight < r.minHeight || atHeight > r.maxHeight) continue;

            float w = Mathf.Max(0f, r.weight);
            if (roll <= w) return r;
            roll -= w;
        }

        // Shouldn’t happen, but keeps things safe.
        return null;
    }
}
