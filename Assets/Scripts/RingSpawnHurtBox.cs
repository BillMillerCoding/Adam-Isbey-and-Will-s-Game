using UnityEngine;

public class RingSpawnHurtBox : MonoBehaviour
{
    private void SpawnHurtBox()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
}
