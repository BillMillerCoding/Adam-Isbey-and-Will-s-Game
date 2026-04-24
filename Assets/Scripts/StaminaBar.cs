using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerMovement playerHealth;
    public GameObject whosHealthBar;

    private void Start()
    {
        playerHealth = whosHealthBar.GetComponent<PlayerMovement>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.MaximumHealth;
        healthBar.value = playerHealth.MaximumHealth;
    }

    public void SetHealth(float hp)
    {
        healthBar.value = hp;
    }
    
    public void SetMaxHealth(float hp)
    {
        healthBar.maxValue = hp;
    }
}