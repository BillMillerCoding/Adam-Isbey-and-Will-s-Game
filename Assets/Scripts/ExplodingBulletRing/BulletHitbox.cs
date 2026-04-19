using UnityEngine;

public class BulletHitbox : MonoBehaviour
{
    //Hitbox for boss exploding bullets. Calls TakeDamage method on
    //PlayerHealth script if the collider of the bullet intersects
    //that of the Player
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealthScript = other.GetComponent<PlayerHealth>();
            if(playerHealthScript != null)
            {
                playerHealthScript.TakeDamage(10);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Wall"))
        {
            //Destroy bullet since it hit a wall collider.
            Destroy(gameObject);
        }
        
    }
}
