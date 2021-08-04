using UnityEngine;

public class SlowTime : PowerUp
{
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float duration = 3;
    protected override void ActivatePowerUp()
    {
        EventManager.InvokeSlowTime(duration, slowFactor);
    }
}