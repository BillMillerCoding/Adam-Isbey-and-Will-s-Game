using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies or hazards from a configurable pool of BossSummonData.
/// No animation is required — the boss simply pauses for castDuration.
/// </summary>
public class BossSummonHandler : MonoBehaviour
{
    // ─── Configuration ───────────────────────────────────────────────────────
    [Header("Summon Pool")]
    [Tooltip("Drag BossSummonData assets here.")]
    [SerializeField] private List<BossSummonData> summons = new List<BossSummonData>();

    // ─── References ──────────────────────────────────────────────────────────
    private Transform playerTarget;

    [Header("Debug (read-only)")]
    [SerializeField] private string lastSummonUsed = "";

    /// <summary>Must be called once by BossController to supply the player reference.</summary>
    public void Initialise(Transform player)
    {
        playerTarget = player;
    }

    // ─── Public API ──────────────────────────────────────────────────────────

    /// <summary>
    /// Picks a valid summon, spawns the prefab, and waits for the cast duration.
    /// Returns false via callback if no valid summon was available.
    /// </summary>
    public IEnumerator ExecuteSummon(System.Action<bool> result)
    {
        BossSummonData chosen = SelectSummon();
        if (chosen == null)
        {
            Debug.LogWarning("[BossSummonHandler] No valid summon available.");
            result?.Invoke(false);
            yield break;
        }

        chosen.lastUsedTime = Time.time;
        lastSummonUsed = chosen.summonName;

        Debug.Log($"[BossSummonHandler] Summoning: {chosen.summonName}");

        // Determine spawn position
        Vector2 spawnPos;
        if (chosen.spawnAtPlayer && playerTarget != null)
            spawnPos = (Vector2)playerTarget.position + chosen.spawnOffset;
        else
            spawnPos = (Vector2)transform.position + chosen.spawnOffset;

        // Instantiate the prefab
        if (chosen.prefab != null)
        {
            Instantiate(chosen.prefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"[BossSummonHandler] Prefab is null for summon: {chosen.summonName}");
        }

        // Wait for cast duration
        yield return new WaitForSeconds(chosen.castDuration);

        result?.Invoke(true);
    }

    // ─── Selection Logic ─────────────────────────────────────────────────────

    private BossSummonData SelectSummon()
    {
        List<BossSummonData> candidates = new List<BossSummonData>();
        float totalWeight = 0f;

        foreach (var s in summons)
        {
            if (!s.IsOffCooldown) continue;
            candidates.Add(s);
            totalWeight += s.weight;
        }

        if (candidates.Count == 0) return null;

        float roll = Random.Range(0f, totalWeight);
        float running = 0f;
        foreach (var s in candidates)
        {
            running += s.weight;
            if (roll <= running)
                return s;
        }

        return candidates[candidates.Count - 1];
    }
}
