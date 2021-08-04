using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private const float MinAngle = 5; //Minimum angle with horizontal
    private const int MaxBalls = 3; //Maximum balls concurently on the screen
    private const int BallLayer = 9; //Ignore collisions between balls
    private const int IgnorePhysicsLayer = 8;
    private static int ballCount = 0; //Current balls on screen
    private static bool isOnFire;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 7;
    private Vector3 lastVelocity;
    private Transform myTransform;
    private readonly float minSin = Mathf.Sin(MinAngle * Mathf.Deg2Rad);

    private PoolID ID => PoolID.Ball;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        myTransform = transform;
    }

    private void OnEnable()
    {
        EventManager.EDoubleBalls += SpawnBalls;
        EventManager.EFireBalls += ActivateFireBalls;
        EventManager.EGameEnded += Deactivate;
    }

    private void OnDisable()
    {
        EventManager.EDoubleBalls -= SpawnBalls;
        EventManager.EFireBalls -= ActivateFireBalls;
        EventManager.EGameEnded -= Deactivate;
    }

    private void LateUpdate()
    {
        if (rb.velocity.magnitude < speed)
            rb.velocity = rb.velocity.normalized * speed;

        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //Send the ball back depending on the hit spot
            rb.velocity = (myTransform.position - collision.transform.position).normalized * speed;
        else if (collision.gameObject.CompareTag("Brick"))
        {
            if (!isOnFire) //Bounce the ball off the brick is power up is inactive
                Bounce(collision.GetContact(0).normal);
            else //Go through brick
                rb.velocity = lastVelocity;
        }
        else //Hit a boundary
            Bounce(collision.GetContact(0).normal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Deactivate();

            if (ballCount == 0)
                EventManager.InvokeLifeLost();
        }
    }

    void Bounce(Vector3 normal)
    {
        rb.velocity = Vector3.Reflect(lastVelocity, normal).normalized * speed; //Reflect based on collision point normal
        CheckAngle();
        
        if (rb.velocity.magnitude == 0) //Prevent ball being stuck in place
            rb.velocity = Vector3.up * speed;
    }

    public void Shoot(Vector3 direction)
    {
        rb.velocity = direction * speed;
        Invoke(nameof(Activate), 0.2f); //Activate collisions after 0.2 seconds
    }

    void Activate()
    {
        gameObject.layer = BallLayer;
        ballCount++;
    }

    void Deactivate()
    { 
        ballCount -= 1;
        gameObject.layer = IgnorePhysicsLayer; //Prevent collision with player while shooting
        rb.velocity = Vector3.zero;
        PoolManager.Instance.AddToPool(ID, gameObject);
    }

    void CheckAngle()
    {
        //Get angle with horizontal and cos and sin
        float angle = Vector3.SignedAngle(Vector3.right, rb.velocity, Vector3.forward) * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        if (Mathf.Abs(sin) < Mathf.Abs(minSin)) //If true ball is barely moving vertically
        {
            if (Mathf.Sign(sin) == Mathf.Sign(cos)) //1 & 3 quadrants rotate CCW
                rb.velocity = Quaternion.Euler(Vector3.forward * MinAngle) * rb.velocity;
            else //2 & 4 quadrants rotate counter CW
                rb.velocity = Quaternion.Euler(Vector3.forward * -MinAngle) * rb.velocity;
        }
    }

    void SpawnBalls() //Spawn additional balls on screen
    {
        for (; ballCount < MaxBalls; ballCount++)
        {
            GameObject ball = PoolManager.Instance.GetPooledObject(ID);
            ball.transform.SetPositionAndRotation(myTransform.position, Quaternion.identity);
            ball.layer = BallLayer;
            ball.GetComponent<Rigidbody>().velocity = Quaternion.Euler(Vector3.forward * Random.Range(-45, 45f)) * rb.velocity;
        }
    }

    void ActivateFireBalls(float duration)
    {
        if (!isOnFire) //Activate fireballs if deactivated
        {
            isOnFire = true;
            DeactivateFireBalls(duration);
        }
    }

    async void DeactivateFireBalls(float duration) //Will deactivate even if ball is inactive
    {
        await System.Threading.Tasks.Task.Delay((int)(duration * 1000));
        isOnFire = false;
    }
}