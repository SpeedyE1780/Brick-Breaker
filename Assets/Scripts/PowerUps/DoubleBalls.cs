public class DoubleBalls : PowerUp
{
    protected override void ActivatePowerUp() => EventManager.InvokeDoubleBalls();
}