using UnityEngine;

public class ExplodingBullet : MonoBehaviour
{
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifetime;//how long before bullet explodes.
    private GameObject bulletPrefab;//prefab of bullet.
    [SerializeField] private int generation;//this determines what generation a bullet is. Generations 1 and 2 spawn more bullets, Gen 3 doesn't.
    [SerializeField] private int maxGeneration = 2;

    private Rigidbody2D rb;//rigidbody of bullet.
    private float timer = 0;//counts up until projectile explodes.

    bool isInitialized = false;

    //Awake runs before start. It provides an opportunity to assign values to variables before starting the
    //game.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        timer += Time.deltaTime;//increments timer with time since last frame.
        if (timer >= lifetime)
        {
            Explode();
        }

    }

    //Initializes the movement direction, generation and prefab of a newly created bullet.
    public void initializeBullet(Vector2 newMoveDirection, int generation, GameObject bulletPrefab)
    {
        this.moveDirection = newMoveDirection;
        this.generation = generation;
        this.bulletPrefab = bulletPrefab;
        rb.linearVelocity = moveDirection * moveSpeed;
        isInitialized = true;
    }

    private void Explode()
    {
        
        int bulletCount = 4;
        float angleStep = 360f / bulletCount;
        float angle = 0;

        if (generation < maxGeneration)
        {
            for (int bulletIndex = 0; bulletIndex < bulletCount; bulletIndex++)
            {
                //calculates bullet angle in degrees then converts to radians.
                angle = bulletIndex * angleStep * Mathf.Deg2Rad;

                //Cos and Sin are used to map angle to perimeter of unit circle surrounding initial bullet.
                Vector2 directionOfBullet = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                if (bulletPrefab == null)
                {
                    Debug.LogError("bulletPrefab is null on generation: " + generation);
                    return;
                }
                GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                ExplodingBullet newBulletScript = newBullet.GetComponent<ExplodingBullet>();
                newBulletScript.initializeBullet(directionOfBullet, generation + 1, bulletPrefab);

                //gameObject is an implicit reference to the current bullet game object.
               

            }
            Destroy(gameObject);//destroy parent bullet after ring of children bullets spawns
        }
        else
        {
            Destroy(gameObject);//once max generation is reached, all bullets are destroyed after timer
        }
    }



}