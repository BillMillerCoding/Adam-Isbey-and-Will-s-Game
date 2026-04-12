using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth;
    
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
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //implement death later. Possibly trigger an animation, but then cut to game over/retry screen which respawns in boss room.
        Debug.Log("player has been killed game over!");
    }

   
}
