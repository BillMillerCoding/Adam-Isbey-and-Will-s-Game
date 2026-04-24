using System;
using UnityEngine;

public class PinkLazerHitboxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pinkLazerHurtboxPrefab;
    [SerializeField] private float spawnLeftUnits = 0f;
    [SerializeField] private float spawnUpUnits = 0f;

    public void PinkLazerStartHurtbox()
    {
        if (pinkLazerHurtboxPrefab == null)
        {
            Debug.LogWarning("PinkLazerHitboxSpawner: No hurtbox prefab assigned.", this);
            return;
        }

        Transform targetTransform = transform.parent != null ? transform.parent : transform;
        Vector3 spawnPosition = targetTransform.position + Vector3.left * spawnLeftUnits + Vector3.up * spawnUpUnits;
        Instantiate(pinkLazerHurtboxPrefab, spawnPosition, targetTransform.rotation);
    }
    public void Kill()
    {
        Console.WriteLine("PinkLazerHitboxSpawner: Destroying self.");
        Destroy(gameObject);
    }
}
