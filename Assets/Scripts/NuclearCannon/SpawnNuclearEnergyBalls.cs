using UnityEngine;

public class SpawnNuclear : MonoBehaviour
{
    [SerializeField] private GameObject bigNuclearEnergyPrefab;
    [SerializeField] private GameObject smallNuclearEnergyPrefab;
    [SerializeField] private Transform leftHandSpawnPoint;
    [SerializeField] private Transform rightHandSpawnPoint;

    public void FireBalls()
    {
        Vector2 leftDirection = new Vector2(-1, 0);
        Vector2 rightDirection = new Vector2(1, 0);
        GameObject leftBall = Instantiate(bigNuclearEnergyPrefab, leftHandSpawnPoint.position, Quaternion.identity);
        GameObject rightball = Instantiate(bigNuclearEnergyPrefab, rightHandSpawnPoint.position, Quaternion.identity);
        ExplodingBullet leftBallScript = leftBall.GetComponent<ExplodingBullet>();
        ExplodingBullet rightBallScript = rightball.GetComponent<ExplodingBullet>();
        leftBallScript.initializeBullet(leftDirection, 0, smallNuclearEnergyPrefab);
        rightBallScript.initializeBullet(rightDirection, 0, smallNuclearEnergyPrefab);

    }
}
