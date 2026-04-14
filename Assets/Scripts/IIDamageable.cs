using UnityEngine;

public interface IIDamageable
{
    public void TakeDamage(float amount);
    float MaximumHealth { get; }
}
