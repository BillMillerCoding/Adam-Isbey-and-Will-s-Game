using UnityEngine;

public class RingSpawnHurtBox : MonoBehaviour
{
    public float damage = 10;
    private void SpawnHurtBox()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the script on the other object
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
        }
    }

}
