using UnityEngine;

/// <summary>
/// Lives on the lazer prefab. Follows the boss's head transform and aims at the player.
/// An animation event can call StopFollowingPlayer() to lock the current aim direction.
/// </summary>
public class LazerController : MonoBehaviour
{
    [Tooltip("Direction the lazer sprite faces by default (degrees). 0 = right, 90 = up, 180 = left, 270 = down.")]
    [SerializeField] private float spriteDefaultAngle = 0f;

    private Transform headTransform;
    private Transform player;
    private bool followingPlayer = true;

    /// <summary>
    /// Called by LazerSpawner right after instantiation to provide the head reference.
    /// </summary>
    public void Init(Transform head)
    {
        headTransform = head;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void LateUpdate()
    {
        // Follow the boss head position
        if (headTransform != null)
            transform.position = headTransform.position;

        // Aim at the player until told to stop
        if (followingPlayer && player != null)
        {
            Vector2 direction = (Vector2)player.position - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - spriteDefaultAngle);
        }
    }

    /// <summary>
    /// Called by an Animation Event to lock the lazer's current aim direction.
    /// </summary>
    public void StopFollowingPlayer()
    {
        followingPlayer = false;
    }
}
