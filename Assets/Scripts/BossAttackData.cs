using UnityEngine;

/// <summary>
/// Defines a single boss attack: animation trigger, preferred range, cooldown, and selection weight.
/// Create instances via Assets > Create > Boss > Attack Data.
/// </summary>
[CreateAssetMenu(fileName = "NewAttack", menuName = "Boss/Attack Data")]
public class BossAttackData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Display name for debugging")]
    public string attackName = "New Attack";

    [Header("Animation")]
    [Tooltip("Animator trigger name to fire when this attack plays")]
    public string animationTrigger = "";

    [Header("Range")]
    [Tooltip("Which distance ring this attack is designed for (0 = Close, 1 = Mid, 2 = Far). " +
             "The attack can still be selected outside this ring if no better option exists.")]
    [Range(0, 2)]
    public int preferredRing = 0;

    [Header("Cooldown")]
    [Tooltip("Minimum seconds between uses of this attack")]
    public float cooldown = 3f;

    [Header("Selection")]
    [Tooltip("Relative weight when multiple attacks are valid candidates. Higher = more likely.")]
    public float weight = 1f;

    [Tooltip("Duration in seconds for the attack coroutine to wait. " +
             "This is a fallback — ideally an Animation Event calls BossAttackHandler.OnAttackAnimationEnd().")]
    public float fallbackDuration = 1f;

    // --- Runtime state (not saved to the asset) ---
    [System.NonSerialized] public float lastUsedTime = -999f;

    /// <summary>Returns true if enough time has passed since the last use.</summary>
    public bool IsOffCooldown => Time.time - lastUsedTime >= cooldown;
}
