using UnityEngine;

/// <summary>
/// Handles boss movement toward the player via Rigidbody2D.
/// Exposes start/stop and a teleport method for the "beyond far" timer.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BossMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [Tooltip("How close the boss gets before stopping its approach")]
    [SerializeField] private float stoppingDistance = 1.5f;

    [Header("Teleport")]
    [Tooltip("Offset from the player's position when teleporting")]
    [SerializeField] private Vector2 teleportOffset = new Vector2(2f, 0f);

    private Rigidbody2D rb;
    private Transform playerTarget;

    [Header("Debug (read-only)")]
    [SerializeField] private bool isMoving = false;

    public bool IsMoving => isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>Must be called once by BossController to supply the player reference.</summary>
    public void Initialise(Transform player)
    {
        playerTarget = player;
    }

    private void FixedUpdate()
    {
        if (playerTarget != null)
        {
            FacePlayer();
        }

        if (!isMoving || playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)playerTarget.position - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, playerTarget.position);

        if (distance > stoppingDistance)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            Debug.Log("[BossMovement] Reached stopping distance.");
        }
    }

    /// <summary>Rotate the boss to face the player. Y rotation 180 if player is left, 0 if right.</summary>
    private void FacePlayer()
    {
        float yRotation = playerTarget.position.x < transform.position.x ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    /// <summary>Begin moving toward the player.</summary>
    public void StartApproach()
    {
        if (playerTarget == null) return;
        isMoving = true;
        Debug.Log("[BossMovement] Approaching player.");
    }

    /// <summary>Halt all movement immediately.</summary>
    public void StopMovement()
    {
        isMoving = false;
        rb.linearVelocity = Vector2.zero;
    }

    /// <summary>Instantly reposition the boss near the player.</summary>
    public void TeleportToPlayer()
    {
        if (playerTarget == null) return;

        Vector2 destination = (Vector2)playerTarget.position + teleportOffset;
        rb.position = destination;
        rb.linearVelocity = Vector2.zero;
        isMoving = false;
        Debug.Log($"[BossMovement] Teleported to {destination}.");
    }
}
