using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
/// <summary>
/// Tracks boss HP and exposes the current phase (1, 2, or 3).
/// Fires UnityEvents when the boss changes phase or dies.
/// </summary>
public class OldBossHealth : MonoBehaviour, IIDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 300f;
    [SerializeField] private float currentHealth;

    [Header("Phase Thresholds (% of max HP)")]
    [Tooltip("Above this % → Phase 1")]
    [SerializeField, Range(0f, 1f)] private float phase2Threshold = 0.66f;
    [Tooltip("Above this % → Phase 2, below → Phase 3")]
    [SerializeField, Range(0f, 1f)] private float phase3Threshold = 0.33f;

    [Header("Events")]
    [Tooltip("Fired when the phase changes. Passes the new phase number (1-3).")]
    public UnityEvent<int> OnPhaseChanged;
    [Tooltip("Fired when HP reaches 0.")]
    public UnityEvent OnDeath;

    [Header("Debug (read-only)")]
    [SerializeField] private int currentPhase = 1;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public int CurrentPhase => currentPhase;
    public bool IsDead => currentHealth <= 0f;
    public float HealthPercent => maxHealth > 0f ? currentHealth / maxHealth : 0f;
    
    //this is for testing
    public HealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentPhase = 1;
        // these two lines are for testing
    }
    /// <summary>
    /// Used to apply damage to the boss.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"[BossHealth] Took {amount} damage. HP: {currentHealth}/{maxHealth}");

        EvaluatePhase();
        healthBar.SetHealth( currentHealth );
        if (IsDead)
        {
            Debug.Log("[BossHealth] Boss has died.");
            OnDeath?.Invoke();
        }
    }

    public float MaximumHealth
    {
        get => MaxHealth;
    }

    /// <summary>
    /// Recalculates the phase from current HP and fires the event if it changed.
    /// </summary>
    private void EvaluatePhase()
    {
        int newPhase;
        float pct = HealthPercent;

        if (pct > phase2Threshold)
            newPhase = 1;
        else if (pct > phase3Threshold)
            newPhase = 2;
        else
            newPhase = 3;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            Debug.Log($"[BossHealth] Phase changed → {currentPhase}");
            OnPhaseChanged?.Invoke(currentPhase);
        }
    }
    
}
