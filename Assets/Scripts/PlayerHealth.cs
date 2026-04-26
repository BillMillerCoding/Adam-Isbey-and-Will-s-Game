using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IIDamageable, IResettable
{
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthBar;
    public UnityEvent OnDeath;
    public GameObject oneUpPrefab;   // assign in Inspector
    public Canvas canvas;            // assign your Screen Space canvas
    private float savedHealth;
    private float savedMaxHealth;
    private AudioSource audioSource;
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        ResetManager.Register(this);
    }

    public void TakeDamage(float damage)
    {
        if ((currentHealth > 0))
        {
            //SpawnPopup("Damage", Color.red);

            currentHealth -= damage;
            if (audioSource != null)
                audioSource.Play();
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
    

    public void SpawnPopup(string message, Color color)
    {
        Vector3 worldPos = transform.position + new Vector3(0, 0, 0);

        GameObject popupObj = Instantiate(oneUpPrefab, canvas.transform);

        PopupFollow popup = popupObj.GetComponent<PopupFollow>();
        popup.cam = Camera.main;
        popup.Initialize(worldPos, message, color);
    }

    public void increaseHealth(float amount)
    {
        currentHealth += amount;
        maxHealth += amount;
        healthBar.SetHealth( currentHealth );
        healthBar.SetMaxHealth( maxHealth );
    }


    public void SaveSnapshot()
    {
        savedHealth = savedHealth == 0? maxHealth : currentHealth;
        savedMaxHealth = maxHealth;
    }

    public void RestoreSnapshot()
    {
        currentHealth = savedHealth;
        maxHealth = savedMaxHealth;
        healthBar.SetHealth( currentHealth );
        healthBar.SetMaxHealth( maxHealth );
    }
}