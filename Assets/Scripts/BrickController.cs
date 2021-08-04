using UnityEngine;

public class BrickController : MonoBehaviour
{
    [SerializeField] private int powerUpChance = 20;
    [SerializeField] private int score = 100;
    private bool addedToPool;

    private static PoolID ID => PoolID.Brick;
    private static int brickCount = 0;

    private void OnEnable()
    {
        brickCount += 1;
        addedToPool = false;
    }

    private void OnDisable() => brickCount -= 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (!addedToPool) //Prevent being added twice to pool in case two collisions occured at the same frame
        {
            addedToPool = true;

            if (Random.Range(0, 100) < powerUpChance) //Randomly spawn power up
                EventManager.InvokeSpawnPower(transform.position);

            EventManager.InvokeAddScore(score);
            PoolManager.Instance.AddToPool(ID, gameObject);

            if (brickCount <= 0) //Check if all bricks were destroyed
                EventManager.InvokeGameEnded();
        }
    }
}