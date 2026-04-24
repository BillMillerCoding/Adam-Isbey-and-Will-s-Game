using UnityEngine;

public class MoveHorizontally : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float startDelay = 0f;

    private float timeSinceSpawn;

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn < startDelay)
        {
            return;
        }

        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
