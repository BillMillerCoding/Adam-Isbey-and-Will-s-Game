using System.Threading.Tasks;
using UnityEngine;

public partial interface IIDamageable
{
    public void TakeDamage(float amount);
    float MaximumHealth { get; }
}