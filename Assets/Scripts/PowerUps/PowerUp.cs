using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private PoolID ID => PoolID.PowerUp;
    protected abstract void ActivatePowerUp();

    private void OnEnable()
    {
        EventManager.ELifeLost += DestroyPowerUp;
        EventManager.EGameEnded += DestroyPowerUp;
    }

    private void OnDisable()
    {
        EventManager.ELifeLost -= DestroyPowerUp;
        EventManager.EGameEnded -= DestroyPowerUp;
    }

    private void DestroyPowerUp() => PoolManager.Instance.AddToPool(ID, gameObject);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Respawn"))
        {
            if (other.CompareTag("Player"))
                ActivatePowerUp();

            PoolManager.Instance.AddToPool(ID, gameObject);
        }
    }
}