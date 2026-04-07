using UnityEngine;

/// <summary>
/// Determines how many attacks and summons the boss performs each decision cycle,
/// based on the current phase provided by BossHealth.
/// </summary>
public class BossPhaseManager : MonoBehaviour
{
    [Header("Phase 1 — Actions per cycle")]
    [Tooltip("Attacks allowed in a single Phase 1 cycle")]
    [SerializeField] private int phase1Attacks = 1;
    [Tooltip("Summons allowed in a single Phase 1 cycle")]
    [SerializeField] private int phase1Summons = 1;
    [Tooltip("If true, boss does EITHER attacks OR summons in Phase 1, not both")]
    [SerializeField] private bool phase1ExclusiveChoice = true;

    [Header("Phase 2 — Actions per cycle")]
    [SerializeField] private int phase2Attacks = 1;
    [SerializeField] private int phase2Summons = 1;
    [SerializeField] private bool phase2ExclusiveChoice = false;

    [Header("Phase 3 — Actions per cycle")]
    [SerializeField] private int phase3Attacks = 1;
    [SerializeField] private int phase3Summons = 2;
    [SerializeField] private bool phase3ExclusiveChoice = false;

    /// <summary>
    /// Returns the action budget for the given phase.
    /// </summary>
    public PhaseActionPlan GetPlan(int phase)
    {
        switch (phase)
        {
            case 1: return new PhaseActionPlan(phase1Attacks, phase1Summons, phase1ExclusiveChoice);
            case 2: return new PhaseActionPlan(phase2Attacks, phase2Summons, phase2ExclusiveChoice);
            case 3: return new PhaseActionPlan(phase3Attacks, phase3Summons, phase3ExclusiveChoice);
            default:
                Debug.LogWarning($"[BossPhaseManager] Unknown phase {phase}, defaulting to Phase 1.");
                return new PhaseActionPlan(phase1Attacks, phase1Summons, phase1ExclusiveChoice);
        }
    }
}

/// <summary>
/// Simple data container describing what the boss should do in one cycle.
/// </summary>
public struct PhaseActionPlan
{
    public int attacks;
    public int summons;
    public bool exclusiveChoice;

    public PhaseActionPlan(int attacks, int summons, bool exclusiveChoice)
    {
        this.attacks = attacks;
        this.summons = summons;
        this.exclusiveChoice = exclusiveChoice;
    }
}
