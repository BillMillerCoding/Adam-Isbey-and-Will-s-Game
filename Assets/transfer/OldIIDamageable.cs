using UnityEngine;

public partial interface OldIIDamageable
{
    public void TakeDamage(float amount);
    float MaximumHealth { get; }
}
