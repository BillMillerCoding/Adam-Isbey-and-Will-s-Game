using UnityEngine;

public class randomattacking : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack();
    }
    void stop()
    {
        animator.SetTrigger("Stop");
    }

    void attack()
    {
        animator.SetTrigger("Attack");
    }
}