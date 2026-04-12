using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private float damage = 20f;
    private Vector2 direction;

    public void Launch(Vector2 launchDirection)
    {
        direction = launchDirection.normalized;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            bossHealth.TakeDamage(damage);
            // deal damage later
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f); // cleanup after 5 seconds
    }
}