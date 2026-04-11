using System.Collections;
using UnityEngine;

public class RingFollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private int NumberOfRings = 5;
    [SerializeField] private float TimeBetweenRings = 1f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnRings());
    }

    private IEnumerator SpawnRings()
    {
        for (int i = 0; i < NumberOfRings; i++)
        {
            Instantiate(ringPrefab, playerTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(TimeBetweenRings);
        }
    }
}
