using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class OldHealth : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 100;
    private InputAction jumpAction;
    public HealthBar healthBar;
    void Awake()
    {
        jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        jumpAction.Enable();
    }

    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            DamagePlayer(10);
        }
    }

    public void DamagePlayer( int damage )
    {
        curHealth -= damage;

        healthBar.SetHealth( curHealth );
    }
}




