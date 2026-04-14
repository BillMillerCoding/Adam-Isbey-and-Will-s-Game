using UnityEngine;

public class BossProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public void SpawnProjectile()
    {
        if (projectilePrefab == null) return;

        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    }
}
