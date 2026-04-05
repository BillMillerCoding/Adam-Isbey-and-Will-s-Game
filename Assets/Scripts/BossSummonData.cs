using UnityEngine;

/// <summary>
/// Defines a single summon action: what prefab to spawn, cooldown, and spawn positioning.
/// Create instances via Assets > Create > Boss > Summon Data.
/// </summary>
[CreateAssetMenu(fileName = "NewSummon", menuName = "Boss/Summon Data")]
public class BossSummonData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Display name for debugging")]
    public string summonName = "New Summon";

    [Header("Prefab")]
    [Tooltip("The enemy or hazard prefab to instantiate")]
    public GameObject prefab;

    [Header("Spawn Positioning")]
    [Tooltip("Offset from the boss position where the prefab spawns")]
    public Vector2 spawnOffset = Vector2.zero;

    [Tooltip("If true, spawn at the player's position instead of relative to the boss")]
    public bool spawnAtPlayer = false;

    [Header("Cooldown")]
    [Tooltip("Minimum seconds between uses of this summon")]
    public float cooldown = 5f;

    [Header("Selection")]
    [Tooltip("Relative weight when multiple summons are valid candidates. Higher = more likely.")]
    public float weight = 1f;

    [Header("Timing")]
    [Tooltip("Seconds the boss pauses during this summon (e.g., for a visual cue)")]
    public float castDuration = 0.5f;

    // --- Runtime state ---
    [System.NonSerialized] public float lastUsedTime = -999f;

    /// <summary>Returns true if enough time has passed since the last use.</summary>
    public bool IsOffCooldown => Time.time - lastUsedTime >= cooldown;
}
