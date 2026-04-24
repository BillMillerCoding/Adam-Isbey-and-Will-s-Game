using UnityEngine;

public class PinkHurtBox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTimeSeconds = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTimeSeconds);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Supports colliders on child objects by checking up the hierarchy.
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
