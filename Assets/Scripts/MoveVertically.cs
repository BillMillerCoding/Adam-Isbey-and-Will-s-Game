using UnityEngine;

public class MoveVertically : MonoBehaviour
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

        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
