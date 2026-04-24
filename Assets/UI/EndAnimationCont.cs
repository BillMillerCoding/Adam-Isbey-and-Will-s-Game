using UnityEngine;

public class EndAnimationCont : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stop();
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
