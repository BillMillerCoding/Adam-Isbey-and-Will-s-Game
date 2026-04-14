using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IIDamageable
{
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthBar;
    public UnityEvent OnDeath;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if ((currentHealth > 0))
        {
            currentHealth -= damage;
        }
        healthBar.SetHealth( currentHealth );
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public float MaximumHealth { get{return maxHealth;}  }

    public void Die()
    {
        //implement death later. Possibly trigger an animation, but then cut to game over/retry screen which respawns in boss room.
        Debug.Log("player has been killed game over!");
        OnDeath.Invoke();
        
    }

   
}
