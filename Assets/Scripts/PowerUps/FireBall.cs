using UnityEngine;

public class FireBall : PowerUp
{
    [SerializeField] private float duration = 5;

    protected override void ActivatePowerUp()
    {
        EventManager.InvokeFireBalls(duration);
    }
}