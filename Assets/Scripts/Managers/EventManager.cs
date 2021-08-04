using UnityEngine;

//Event class that invokes all functions related to an event when it occurs
public class EventManager
{
    public delegate void AddScore(int points); //Whenever a brick is destroyed
    public static event AddScore EAddScore;
    public static void InvokeAddScore(int points) => EAddScore?.Invoke(points);

    public delegate void LifeLost(); //When all the balls goes off the screen
    public static event LifeLost ELifeLost;
    public static void InvokeLifeLost() => ELifeLost?.Invoke();

    public delegate void SpawnPowerUp(Vector3 spawnPosition); //When a brick spawns a power up
    public static event SpawnPowerUp ESpawnPowerUp;
    public static void InvokeSpawnPower(Vector3 spawnPosition) => ESpawnPowerUp?.Invoke(spawnPosition);

    public delegate void DoubleSize(float duration); //When double size power up is picked up
    public static event DoubleSize EDoubleSize;
    public static void InvokeDoubleSize(float duration) => EDoubleSize?.Invoke(duration);

    public delegate void SlowTime(float duration, float factor); //When slow time power up is picked up
    public static event SlowTime ESlowTime;
    public static void InvokeSlowTime(float duration, float factor) => ESlowTime?.Invoke(duration, factor);

    public delegate void DoubleBalls(); //When double power up is picked up
    public static event DoubleBalls EDoubleBalls;
    public static void InvokeDoubleBalls() => EDoubleBalls?.Invoke();

    public delegate void FireBalls(float duration); //When fire ball power up is picked up
    public static event FireBalls EFireBalls;
    public static void InvokeFireBalls(float duration) => EFireBalls?.Invoke(duration);

    public delegate void Laser(float duration); //When laser power up is picked up
    public static event Laser ELaser;
    public static void InvokeLaser(float duration) => ELaser?.Invoke(duration);

    public delegate void GameEnded(); //When all bricks or all lifes are gone
    public static event GameEnded EGameEnded;
    public static void InvokeGameEnded() => EGameEnded?.Invoke();
}