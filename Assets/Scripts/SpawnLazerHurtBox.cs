using UnityEngine;

public class SpawnLazerHurtBox : MonoBehaviour
{
    private Collider2D childCollider;

    private void Awake()
    {
        childCollider = GetComponentInChildren<Collider2D>();
    }

    public void EnableHurtBox()
    {
        if (childCollider != null)
        {
            childCollider.enabled = true;
        }
    }

    public void DisableHurtBox()
    {
        if (childCollider != null)
        {
            childCollider.enabled = false;
        }
    }
    public void PlaySound()
    {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
    }
}
