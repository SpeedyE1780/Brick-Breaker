using UnityEngine;

public class LaserController : MonoBehaviour
{
    private const int IgnorePhysics = 8;
    private const int LaserLayer = 11; //Only collides with bricks and boundaries

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    private PoolID ID => PoolID.Laser;

    private void OnEnable()
    {
        InitializeLaser();
        EventManager.EGameEnded += DestroyLaser;
    }

    private void OnDisable() => EventManager.EGameEnded -= DestroyLaser;

    private void OnCollisionEnter(Collision collision) => PoolManager.Instance.AddToPool(ID, gameObject);

    void InitializeLaser()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.up * speed;

        //Ignore all collisions for 0.2 seconds to prevent laser colliding with player and being destroyed
        gameObject.layer = IgnorePhysics;
        Invoke(nameof(Activate), 0.2f);
    }

    void Activate() => gameObject.layer = LaserLayer;

    void DestroyLaser() => PoolManager.Instance.AddToPool(ID, gameObject);
}