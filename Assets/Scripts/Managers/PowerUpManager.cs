using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [SerializeField] private float powerUpSpeed = 3.5f;

    private void OnEnable() => EventManager.ESpawnPowerUp += SpawnPowerUp;

    private void OnDisable() => EventManager.ESpawnPowerUp -= SpawnPowerUp;

    private void SpawnPowerUp(Vector3 spawnPosition)
    {
        GameObject powerUp = PoolManager.Instance.GetPooledObject(PoolID.PowerUp);
        powerUp.transform.position = spawnPosition;
        powerUp.GetComponent<Rigidbody>().velocity = Vector3.down * powerUpSpeed;
    }
}