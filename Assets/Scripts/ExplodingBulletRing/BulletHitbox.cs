using UnityEngine;

public class BulletHitbox : MonoBehaviour
{
    //Hitbox for boss exploding bullets. Calls TakeDamage method on
    //PlayerHealth script if the collider of the bullet intersects
    //that of the Player

    [SerializeField] private int damage;
    [SerializeField] private AudioClip triggerSound;
    [SerializeField] [Range(0f, 1f)] private float triggerSoundVolume = 1f;

    private void PlayTriggerSound()
    {
        if (triggerSound == null)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(triggerSound, transform.position, triggerSoundVolume);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealthScript = other.GetComponent<PlayerHealth>();
            if(playerHealthScript != null)
            {
                PlayTriggerSound();
                playerHealthScript.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Wall"))
        {
            ExplodingBullet explodingBulletScript = GetComponent<ExplodingBullet>();
            //Explode the bullet since it hit a wall.
            PlayTriggerSound();
            explodingBulletScript.hitWallExplode();
        }
        
    }
}
