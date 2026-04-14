using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public IIDamageable playerHealth;
    public GameObject whosHealthBar;

    private void Start()
    {
        playerHealth = whosHealthBar.GetComponent<IIDamageable>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.MaximumHealth;
        healthBar.value = playerHealth.MaximumHealth;
    }

    public void SetHealth(float hp)
    {
        healthBar.value = hp;
    }
}