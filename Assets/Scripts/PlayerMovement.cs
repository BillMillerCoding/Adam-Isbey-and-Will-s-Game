using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IResettable
{
    public float moveSpeed = 5f;
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.3f;//length of time dodge action in effect
    public int maxEndurance = 3;//endurance will be spent to perform a dodgeroll.
    public float enduranceRechargeTime = 2f;//amount of time needed to recover one unit of endurance
    private CapsuleCollider2D playerCollider;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Vector2 dodgeDirection;
    private Vector2 aimDirection;

    public int currentEndurance;
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float rechargeTimer = 0f;
    public StaminaBar staminaBar;

    public GameObject projectilePrefab;
    public Transform swordTip;

    public SwordHitBox swordHitbox;// this is to grab a reference to the player SwordHitbox child object to
                                   // manage its collider via an animation event.
    private int savedEndurance;
    private int savedMaxEndurance;
    private float savedRechage;
                                    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();//rigid body controls velocity in 2d space.
        animator = GetComponent<Animator>();//manages which animations play
        lastMoveDirection = Vector2.down; //sets default direction of player character to south (0,-1)
        currentEndurance = maxEndurance;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
        ResetManager.Register(this);

    }

    public void FireProjectile()
    {
        if(projectilePrefab != null && swordTip != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePosition.z = 0;
            aimDirection = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;//rotates projectile to match crosshair

            GameObject proj = Instantiate(projectilePrefab, swordTip.position, Quaternion.Euler(0, 0, angle));
            proj.GetComponent<Projectile>().Launch(aimDirection);
        }
    }

    //called by the PlayerInputActions Player asset to record WASD inputs from the user.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    public void OnDodge(InputValue value)
    {
        if(value.isPressed && currentEndurance > 0 && !isDodging)
        {
            isDodging = true;
            dodgeDirection = lastMoveDirection;
            dodgeTimer = dodgeDuration;
            currentEndurance--;
            TakeDamage(1);
            animator.SetBool("isDodging", true);
            // To start I-frames
            gameObject.layer = LayerMask.NameToLayer("IgnoreDamage");
            //playerCollider.enabled = false;//disable collider to give player invincibility frames.
        }
    }

    public void EnableHitbox()//called by animation event to activate SwordHitBox collider.
    {
        swordHitbox.EnableHitbox();
    }

    public void DisableHitbox()//called by an animation event to deactivate SwordHitBox collider.
    {
        swordHitbox.DisableHitbox();
    }




    //called on a timer and applies moveInput updates to the rigidbody of the player character.
    void FixedUpdate()
    {
        //Mouse position is calculated in screen space (bottom left of screen using pixel count), ScreenToWorld()
        //converts this value to the world space. THat is the mouse location is put in terms of world origin.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;//ScreenToWorld returns a point with 3 coordinates
        aimDirection = mousePosition - transform.position;
        
        if (isDodging)//dodging overrides attack layer and base movement layer
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;//increases speed in direction of dodge
            dodgeTimer -= Time.fixedDeltaTime;//begins counting down dodge timer
            if (dodgeTimer <= 0)//when dodge duration ends, set isDodging to false.
            {
                isDodging = false;
                animator.SetBool("isDodging", false);
                // To end I-frames
                gameObject.layer = LayerMask.NameToLayer("Player");
                //playerCollider.enabled = true;
            }
        }

        else//attack overrides base movement layer
        {
            rb.linearVelocity = moveInput * moveSpeed;//these two lines handle player running.
            animator.SetFloat("Speed", moveInput.magnitude); //.magnitude applies pythagorean theorem to the coordinates of moveInput.

            if (Mouse.current.leftButton.wasReleasedThisFrame)//handles attack animations
            {
                animator.SetFloat("MoveX", aimDirection.x);//attack animation is based on direction of crosshair
                animator.SetFloat("MoveY", aimDirection.y);
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
                animator.SetFloat("MoveX", lastMoveDirection.x);//return to WASD directionality
                animator.SetFloat("MoveY", lastMoveDirection.y);
            }
        }

        if (currentEndurance < maxEndurance)
        {
            rechargeTimer += Time.fixedDeltaTime;
            if(rechargeTimer >= enduranceRechargeTime)
            {
                currentEndurance++;
                TakeDamage(1);
                rechargeTimer = 0f;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        staminaBar.SetHealth( currentEndurance );
    }

    public float MaximumHealth
    {
        get { return (float)maxEndurance; }
    }

    public void increaseEndurance(int amount)
    {
        maxEndurance += amount;
        staminaBar.SetMaxHealth( maxEndurance );
    }

    public void SaveSnapshot()
    {
        savedEndurance = savedEndurance == 0 ? maxEndurance : currentEndurance;
        savedMaxEndurance = maxEndurance;
        savedRechage = enduranceRechargeTime;
    }

    public void RestoreSnapshot()
    {
        currentEndurance = savedEndurance;
        maxEndurance = savedMaxEndurance;
        enduranceRechargeTime = savedRechage;
        staminaBar.SetMaxHealth( maxEndurance );
        staminaBar.SetHealth( currentEndurance );
    }
}
