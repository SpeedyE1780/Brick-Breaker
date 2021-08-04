using UnityEngine;

public class DoubleSize : PowerUp
{
    [SerializeField] private float duration = 3;
    protected override void ActivatePowerUp()
    {
        EventManager.InvokeDoubleSize(duration);
    }
}