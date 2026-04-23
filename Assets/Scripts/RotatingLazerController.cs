using UnityEngine;

public class RotatingLazerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private float destroyDelaySeconds = 2f;

    void StopFollowingPlayer()
    {
    }
    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
    void DestroySelf()
    {
        Destroy(gameObject, destroyDelaySeconds);
    }
}
