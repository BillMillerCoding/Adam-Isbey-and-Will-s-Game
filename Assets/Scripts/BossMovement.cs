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
    private SpriteRenderer spriteRenderer;
    [Header("References")]
    [SerializeField] private Transform firePoint;

    private Vector3 firePointBaseLocalPos;

    [Header("Debug (read-only)")]
    [SerializeField] private bool isMoving = false;

    public bool IsMoving => isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Cache the fire point's original local position so we can mirror it when facing changes.
        if (firePoint == null)
        {
            Transform[] children = GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name == "FirePoint")
                {
                    firePoint = children[i];
                    break;
                }
            }
        }

        if (firePoint != null)
        {
            firePointBaseLocalPos = firePoint.localPosition;
        }
    }

    /// <summary>Must be called once by BossController to supply the player reference.</summary>
    public void Initialise(Transform player)
    {
        playerTarget = player;
    }

    private void FixedUpdate()
    {
        if (!isMoving || playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        FacePlayer();

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

    /// <summary>Flip the sprite to face the player using SpriteRenderer.flipX instead of rotation.</summary>
    private void FacePlayer()
    {
        if (spriteRenderer == null) return;
        bool facingLeft = playerTarget.position.x < transform.position.x;
        spriteRenderer.flipX = facingLeft;

        if (firePoint != null)
        {
            float mirroredX = facingLeft ? -Mathf.Abs(firePointBaseLocalPos.x) : Mathf.Abs(firePointBaseLocalPos.x);
            firePoint.localPosition = new Vector3(mirroredX, firePointBaseLocalPos.y, firePointBaseLocalPos.z);
        }
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
