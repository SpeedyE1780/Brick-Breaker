using UnityEngine;

public class Laser : PowerUp
{
    [SerializeField] float duration = 2;
    protected override void ActivatePowerUp()
    {
        EventManager.InvokeLaser(duration);
    }
}