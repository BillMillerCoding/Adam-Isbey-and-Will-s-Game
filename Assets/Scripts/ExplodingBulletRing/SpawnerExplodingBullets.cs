using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class SpawnerExplodingBullets : MonoBehaviour
{
    //This script goes on the boss and is used to spawn the initial right of exploding bullets around the boss.
    
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private int bulletCount;

    public void SpawnRing()
    {
        float angleStep = 360f / bulletCount;
        for(int bulletIndex = 0; bulletIndex < bulletCount; bulletIndex++)
        {
            float angle = bulletIndex * angleStep * Mathf.Deg2Rad;
            Vector2 newBulletDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            if (bulletPrefab == null)
            {
                Debug.LogError("bulletPrefab is null on generation 0");
                return;
            }
            //spawns a new bullet and fires it
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            ExplodingBullet explodingBulletScript = newBullet.GetComponent<ExplodingBullet>();

            //Initial bullet ring spawns on boss transform and is of generation 0. This initialize call is to
            //set up the children of the current new bullet.
            explodingBulletScript.initializeBullet(newBulletDirection, 0, bulletPrefab);
        }
    }
}
