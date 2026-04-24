using UnityEngine;
//Test to merge.
public class SwordHitBox : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D other)//when sword box collider makes contact with boss collider, boss takes damage
    {
        if (other.CompareTag("Boss"))//checks to see if tag of other collider has is "Boss"
        {
            BossHealth enemyHealth = other.GetComponent<BossHealth>();//retrieve boss health script
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);//call TakeDamage method from boss health script.
            }
        }
    }

    public void EnableHitbox()//animation event method for spawning hitbox
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void DisableHitbox()//animation event method for despawning hitbox
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
