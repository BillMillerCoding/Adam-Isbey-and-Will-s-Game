using UnityEngine;

public class BulletHitbox : MonoBehaviour
{
    //Hitbox for boss exploding bullets. Calls TakeDamage method on
    //PlayerHealth script if the collider of the bullet intersects
    //that of the Player

    [SerializeField] private int damage;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealthScript = other.GetComponent<PlayerHealth>();
            if(playerHealthScript != null)
            {
                playerHealthScript.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Wall"))
        {
            ExplodingBullet explodingBulletScript = GetComponent<ExplodingBullet>();
            //Explode the bullet since it hit a wall.
            explodingBulletScript.hitWallExplode();
        }
        
    }
}
