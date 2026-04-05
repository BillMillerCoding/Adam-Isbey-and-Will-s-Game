using System.Collections;
using UnityEngine;

/// <summary>
/// Main boss brain. Runs the state-driven decision loop via coroutine.
/// Coordinates BossHealth, BossPhaseManager, BossMovement, BossAttackHandler,
/// and BossSummonHandler. Does NOT use Update() for logic — everything is
/// coroutine-driven.
/// </summary>
public class BossController : MonoBehaviour
{
    // ─── State ────────────────────────────────────────────────────────────────
    private enum BossState { Idle, Deciding, Approaching, Attacking, Summoning, Dead }

    [Header("Current State (read-only)")]
    [SerializeField] private BossState currentState = BossState.Idle;

    // ─── References ──────────────────────────────────────────────────────────
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Subsystems (auto-wired if on same GameObject)")]
    [SerializeField] private BossHealth health;
    [SerializeField] private BossPhaseManager phaseManager;
    [SerializeField] private BossMovement movement;
    [SerializeField] private BossAttackHandler attackHandler;
    [SerializeField] private BossSummonHandler summonHandler;

    // ─── Distance Rings ──────────────────────────────────────────────────────
    [Header("Distance Rings (units)")]
    [Tooltip("Distance ≤ this → Close ring")]
    [SerializeField] private float closeRange = 3f;
    [Tooltip("Distance ≤ this (but > closeRange) → Mid ring")]
    [SerializeField] private float midRange = 7f;
    [Tooltip("Distance ≤ this (but > midRange) → Far ring")]
    [SerializeField] private float farRange = 12f;

    // ─── Ring Probabilities ──────────────────────────────────────────────────
    [Header("Ring Attack Probabilities")]
    [Tooltip("Chance to attack (vs approach) when in Close ring. 1 = always attack.")]
    [SerializeField, Range(0f, 1f)] private float closeAttackChance = 1f;
    [Tooltip("Chance to attack when in Mid ring")]
    [SerializeField, Range(0f, 1f)] private float midAttackChance = 0.66f;
    [Tooltip("Chance to attack when in Far ring")]
    [SerializeField, Range(0f, 1f)] private float farAttackChance = 0.33f;

    // ─── Beyond-Far Teleport ─────────────────────────────────────────────────
    [Header("Beyond-Far Teleport")]
    [Tooltip("Seconds the player must stay beyond farRange before the boss teleports")]
    [SerializeField] private float beyondFarTeleportDelay = 4f;

    // ─── Timing ──────────────────────────────────────────────────────────────
    [Header("Decision Loop Timing")]
    [Tooltip("Seconds to wait between decision cycles")]
    [SerializeField] private float decisionCooldown = 1f;
    [Tooltip("Seconds the boss moves toward the player per approach step")]
    [SerializeField] private float approachDuration = 1f;

    // ─── Runtime ─────────────────────────────────────────────────────────────
    private float beyondFarTimer = 0f;

    // ─────────────────────────────────────────────────────────────────────────
    // Lifecycle
    // ─────────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        // Auto-wire subsystems from this GameObject if not assigned
        if (health == null) health = GetComponent<BossHealth>();
        if (phaseManager == null) phaseManager = GetComponent<BossPhaseManager>();
        if (movement == null) movement = GetComponent<BossMovement>();
        if (attackHandler == null) attackHandler = GetComponent<BossAttackHandler>();
        if (summonHandler == null) summonHandler = GetComponent<BossSummonHandler>();
    }

    private void Start()
    {
        // Initialise subsystems that need the player reference
        movement.Initialise(player);
        summonHandler.Initialise(player);

        // Subscribe to death event to stop the loop
        health.OnDeath.AddListener(OnBossDied);

        // Start the decision loop
        SetState(BossState.Idle);
        StartCoroutine(DecisionLoop());
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDeath.RemoveListener(OnBossDied);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Decision Loop (coroutine, not Update)
    // ─────────────────────────────────────────────────────────────────────────
    private IEnumerator DecisionLoop()
    {
        // Small initial delay so all systems can initialise
        yield return new WaitForSeconds(0.2f);

        while (!health.IsDead)
        {
            SetState(BossState.Deciding);

            // 1. Current phase
            int phase = health.CurrentPhase;

            // 2. Distance → ring
            int ring = GetCurrentRing(out float distance);

            // 3. Handle "beyond far" teleport timer
            if (ring == 3) // beyond far
            {
                beyondFarTimer += decisionCooldown; // approximate — timer ticks each cycle
                if (beyondFarTimer >= beyondFarTeleportDelay)
                {
                    Debug.Log("[BossController] Teleporting — player stayed beyond far too long.");
                    movement.TeleportToPlayer();
                    beyondFarTimer = 0f;
                    yield return new WaitForSeconds(decisionCooldown);
                    continue;
                }
                // Still beyond far but timer hasn't elapsed — approach
                yield return StartCoroutine(DoApproach());
                yield return new WaitForSeconds(decisionCooldown);
                continue;
            }
            else
            {
                beyondFarTimer = 0f;
            }

            // 4. Roll probability: attack or approach?
            bool shouldAttack = RollAttackChance(ring);

            if (!shouldAttack)
            {
                // Approach
                yield return StartCoroutine(DoApproach());
                yield return new WaitForSeconds(decisionCooldown);
                continue;
            }

            // 5. Get the phase plan
            PhaseActionPlan plan = phaseManager.GetPlan(phase);

            // 6. Resolve exclusive choice for Phase 1 style phases
            int attacksToPerform = plan.attacks;
            int summonsToPerform = plan.summons;

            if (plan.exclusiveChoice)
            {
                // Randomly pick attacks OR summons, not both
                if (Random.value < 0.5f)
                    summonsToPerform = 0;
                else
                    attacksToPerform = 0;
            }

            // 7. Execute actions in sequence — movement stops during attacks
            movement.StopMovement();

            for (int i = 0; i < attacksToPerform; i++)
            {
                yield return StartCoroutine(DoAttack(ring));
            }

            for (int i = 0; i < summonsToPerform; i++)
            {
                yield return StartCoroutine(DoSummon());
            }

            // 8. Wait, then repeat
            SetState(BossState.Idle);
            yield return new WaitForSeconds(decisionCooldown);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Action coroutines
    // ─────────────────────────────────────────────────────────────────────────
    private IEnumerator DoApproach()
    {
        SetState(BossState.Approaching);
        movement.StartApproach();
        yield return new WaitForSeconds(approachDuration);
        movement.StopMovement();
    }

    private IEnumerator DoAttack(int ring)
    {
        SetState(BossState.Attacking);
        bool success = false;
        yield return StartCoroutine(attackHandler.ExecuteAttack(ring, s => success = s));
        if (!success)
            Debug.Log("[BossController] Attack step skipped — no valid attack.");
    }

    private IEnumerator DoSummon()
    {
        SetState(BossState.Summoning);
        bool success = false;
        yield return StartCoroutine(summonHandler.ExecuteSummon(s => success = s));
        if (!success)
            Debug.Log("[BossController] Summon step skipped — no valid summon.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the ring index: 0 = Close, 1 = Mid, 2 = Far, 3 = BeyondFar.
    /// </summary>
    private int GetCurrentRing(out float distance)
    {
        distance = player != null
            ? Vector2.Distance(transform.position, player.position)
            : 0f;

        if (distance <= closeRange) return 0;
        if (distance <= midRange) return 1;
        if (distance <= farRange) return 2;
        return 3; // beyond far
    }

    private bool RollAttackChance(int ring)
    {
        float chance = ring switch
        {
            0 => closeAttackChance,
            1 => midAttackChance,
            2 => farAttackChance,
            _ => 0f
        };
        return Random.value <= chance;
    }

    private void SetState(BossState newState)
    {
        if (currentState != newState)
        {
            Debug.Log($"[BossController] {currentState} → {newState}");
            currentState = newState;
        }
    }

    private void OnBossDied()
    {
        SetState(BossState.Dead);
        movement.StopMovement();
        StopAllCoroutines();
        Debug.Log("[BossController] Boss defeated — decision loop stopped.");
    }
}
