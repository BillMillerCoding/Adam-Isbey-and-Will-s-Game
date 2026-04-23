using UnityEngine;

public class LazerHurtBox : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth == null)
                playerHealth = other.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit by lazer! Damage: " + damage);
            }
            else
            {
                Debug.LogWarning("[LazerHurtBox] Hit object tagged Player but no PlayerHealth component was found.");
            }
        }
    }
}
