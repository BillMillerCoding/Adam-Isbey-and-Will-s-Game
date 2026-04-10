using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.3f;
    public int maxEndurance = 3;//endurance will be spent to perform a dodgeroll.
    public float enduranceRechargeTime = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Vector2 dodgeDirection;

    private int currentEndurance;
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float rechargeTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastMoveDirection = Vector2.down; //sets default direction of player character to south (0,-1)
        currentEndurance = maxEndurance;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);

    }

    //called by the PlayerInputActions Player asset to record WASD inputs from the user.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }

        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    public void OnDodge(InputValue value)
    {
        if(value.isPressed && currentEndurance > 0 && !isDodging)
        {
            isDodging = true;
            dodgeDirection = lastMoveDirection;
            dodgeTimer = dodgeDuration;
            currentEndurance--;
            animator.SetBool("isDodging", true);
        }
    }



    //called on a timer and applies moveInput updates to the rigidbody of the player character.
    void FixedUpdate()
    {
        if (isDodging)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;
            dodgeTimer -= Time.fixedDeltaTime;
            if (dodgeTimer <= 0)
            {
                isDodging = false;
                animator.SetBool("isDodging", false);
            }
        }

        else
        {
            rb.linearVelocity = moveInput * moveSpeed;//these two lines handle player running.
            animator.SetFloat("Speed", moveInput.magnitude); //.magnitude applies pythagorean theorem to the coordinates of moveInput.

            if (Mouse.current.leftButton.isPressed)//handles attack animations
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }

        if (currentEndurance < maxEndurance)
        {
            rechargeTimer += Time.fixedDeltaTime;
            if(rechargeTimer >= enduranceRechargeTime)
            {
                currentEndurance++;
                rechargeTimer = 0f;
            }
        }
    }
}
