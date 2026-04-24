using UnityEngine;

public class SpawnSpinningLazerController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject lazerPrefab;
    public void SpawnLazers()
    {
        if (firePoint == null || lazerPrefab == null)
        {
            Debug.LogWarning("SpawnSpinningLazerController is missing firePoint or lazerPrefab reference.", this);
            return;
        }

        float[] rotations = { 0f, 90f, 180f, 270f };

        foreach (float zRotation in rotations)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, zRotation);
            Instantiate(lazerPrefab, firePoint.position, rotation);
        }
    }
}
