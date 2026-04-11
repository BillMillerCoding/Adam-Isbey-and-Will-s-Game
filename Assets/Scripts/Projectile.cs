using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    public void Launch(Vector2 launchDirection)
    {
        direction = launchDirection.normalized;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // deal damage later
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f); // cleanup after 5 seconds
    }
}