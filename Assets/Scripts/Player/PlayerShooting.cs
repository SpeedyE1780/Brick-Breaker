using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private const int MaxAngle = 85;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform laserParent;
    [SerializeField] private float laserDelay = 0.25f;

    private Movement movement;
    private Transform myTransform;
    private List<Transform> lasers;
    private Vector3[] defaultPoints;
    private bool lasersActivated;

    private PoolID Lasers => PoolID.Laser;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;


        lasers = new List<Transform>();
        foreach (Transform child in laserParent)
            lasers.Add(child);

        lasersActivated = false;
        myTransform = transform;
        defaultPoints = new Vector3[] { Vector3.zero, Vector3.zero };
    }

    private void Start() => movement = GetComponent<Movement>();

    private void OnEnable()
    {
        EventManager.ELaser += ActivateLasers;
        EventManager.EGameEnded += DeactivateLasers;
    }

    private void OnDisable()
    {
        EventManager.ELaser -= ActivateLasers;
        EventManager.EGameEnded -= DeactivateLasers;
    }

    public void StartShoot() => StartCoroutine(Shoot());

    IEnumerator Shoot()
    {
        //Stop movement and show targeting line
        movement.enabled = false;
        line.enabled = true;
        line.SetPositions(defaultPoints);

        //Spawn ball and place it on player
        BallMovement ball = PoolManager.Instance.GetPooledObject(PoolID.Ball).GetComponent<BallMovement>();
        ball.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        Vector3 direction = Vector3.up;
        Vector3 tapPosition;

        while (Input.GetMouseButton(0))
        {
            tapPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            tapPosition.z = 0;

            if (Vector3.Angle(Vector3.up, tapPosition - spawnPoint.position) < MaxAngle) //Prevents player from shooting ball too horizontally or downwards
            {
                direction = tapPosition - spawnPoint.position;
                line.SetPosition(1, myTransform.InverseTransformPoint(tapPosition));
            }

            yield return null;
        }

        //Enables movement and hide targeting line
        movement.enabled = true;
        line.enabled = false;

        //Shoot ball in xy direction
        direction.z = 0;
        ball.Shoot(direction.normalized);
    }

    void ActivateLasers(float duration)
    {
        if (!lasersActivated) //Activate lasers is deactive
        {
            lasersActivated = true;
            StartCoroutine(ShootLasers(duration));
        }
    }

    private void DeactivateLasers() => lasersActivated = false;

    IEnumerator ShootLasers(float duration)
    {
        float startTime = Time.time;
        float deadline = startTime + duration;

        while (lasersActivated && Time.time < deadline) //Keep running until deadline
        {
            foreach (Transform laser in lasers)
            {
                GameObject l = PoolManager.Instance.GetPooledObject(Lasers);
                l.transform.SetPositionAndRotation(laser.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(laserDelay);
        }

        lasersActivated = false;
    }
}