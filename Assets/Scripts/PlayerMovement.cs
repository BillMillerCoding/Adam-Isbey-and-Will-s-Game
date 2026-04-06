using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastMoveDirection = Vector2.down; //sets default direction of player character to south (0,-1)
        animator.SetFloat("Speed", 0);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);

    }

    //called by the PlayerInputActions Player asset to record WASD inputs from the user.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("moveInput: " + moveInput + " Speed: " + moveInput.magnitude);
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }

        Debug.Log("MoveX: " + lastMoveDirection.x + " MoveY: " + lastMoveDirection.y);

        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    //called on a timer and applies moveInput updates to the rigidbody of the player character.
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        //animator.SetFloat("Speed", moveInput.magnitude); //.magnitude applies pythagorean theorem to the coordinates of moveInput.
    }
}
