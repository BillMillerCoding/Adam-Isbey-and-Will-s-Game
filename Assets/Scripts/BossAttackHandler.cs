using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects a valid attack from the data list (respecting cooldowns and preferred ring),
/// plays the associated Animator trigger, and waits for the animation to complete.
/// Animation Events should call OnAttackAnimationEnd() to signal completion;
/// a fallback timer is used if no event fires.
/// </summary>
public class BossAttackHandler : MonoBehaviour
{
    [Header("Attack Pool")]
    [Tooltip("Drag BossAttackData assets here. Order does not matter — selection is weighted.")]
    [SerializeField] private List<BossAttackData> attacks = new List<BossAttackData>();

    private Animator animator;

    private bool attackAnimationFinished;

    [Header("Debug (read-only)")]
    [SerializeField] private string lastAttackUsed = "";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Picks a valid attack for the given ring, executes it, and yields until it finishes.
    /// Returns false if no valid attack was available.
    /// </summary>
    public IEnumerator ExecuteAttack(int currentRing, System.Action<bool> result)
    {
        BossAttackData chosen = SelectAttack(currentRing);
        if (chosen == null)
        {
            Debug.LogWarning("[BossAttackHandler] No valid attack available.");
            result?.Invoke(false);
            yield break;
        }

        chosen.lastUsedTime = Time.time;
        lastAttackUsed = chosen.attackName;
        attackAnimationFinished = false;

        Debug.Log($"[BossAttackHandler] Executing attack: {chosen.attackName}");

        if (animator != null && !string.IsNullOrEmpty(chosen.animationTrigger))
        {
            animator.SetTrigger(chosen.animationTrigger);
        }

        float elapsed = 0f;
        while (!attackAnimationFinished && elapsed < chosen.fallbackDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!attackAnimationFinished)
        {
            Debug.Log($"[BossAttackHandler] Fallback timer expired for {chosen.attackName}.");
        }

        result?.Invoke(true);
    }

    /// <summary>
    /// Call this from an Animation Event on the attack clip to signal completion.
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        attackAnimationFinished = true;
    }

    private BossAttackData SelectAttack(int currentRing)
    {
        List<BossAttackData> candidates = new List<BossAttackData>();
        float totalWeight = 0f;

        foreach (var atk in attacks)
        {
            if (!atk.IsOffCooldown) continue;

            float w = atk.weight;
            if (atk.preferredRing == currentRing)
                w *= 2f;

            candidates.Add(atk);
            totalWeight += w;
        }

        if (candidates.Count == 0) return null;

        float roll = Random.Range(0f, totalWeight);
        float running = 0f;
        foreach (var atk in candidates)
        {
            float w = atk.weight;
            if (atk.preferredRing == currentRing) w *= 2f;
            running += w;
            if (roll <= running)
                return atk;
        }

        return candidates[candidates.Count - 1];
    }
}
