using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int numberOfLifes = 2;
    [SerializeField] private PlayerShooting player;
    [SerializeField] private float timeTransitionSpeed = 1;
    [SerializeField] private SpawnBricks brickSpawner;

    private float slowMotionDuration;

    private void OnEnable()
    {
        EventManager.ELifeLost += LifeLost;
        EventManager.EGameEnded += GameEnded;
        EventManager.ESlowTime += ActivateSlowMotion;
    }

    private void OnDisable()
    {
        EventManager.ELifeLost -= LifeLost;
        EventManager.EGameEnded -= GameEnded;
        EventManager.ESlowTime -= ActivateSlowMotion;
    }

    private void Start()
    {
        player.StartShoot();
        UIManager.Instance.SetLife(numberOfLifes);
    }

    private void GameEnded() => UIManager.Instance.ShowEndMenu(numberOfLifes < 0);

    public void PlayAgain()
    {
        if (numberOfLifes >= 0) //Player won
        {
            brickSpawner.AddBricks();
            numberOfLifes += 1;
        }
        else //Player lost
        {
            numberOfLifes = 2;
            ScoringManager.Instance.ResetScore();
        }

        //Start level
        brickSpawner.PlaceBricks();
        player.StartShoot();
        UIManager.Instance.SetLife(numberOfLifes);
        UIManager.Instance.HideEndMenu();
    }

    private void LifeLost()
    {
        numberOfLifes -= 1;
        UIManager.Instance.SetLife(numberOfLifes);

        if (numberOfLifes < 0) //Player lost
            EventManager.InvokeGameEnded();
        else //Shoot next ball
            player.StartShoot();
    }

    private void ActivateSlowMotion(float duration, float factor)
    {
        if (slowMotionDuration <= 0) //Start coroutine if not active
            StartCoroutine(SlowTime(factor));

        slowMotionDuration += duration; //Set/extend slow motion duration
    }

    IEnumerator SlowTime(float factor)
    {
        yield return StartCoroutine(ScaleTime(factor)); //Slow down time

        while (slowMotionDuration > 0)
        {
            slowMotionDuration -= Time.unscaledDeltaTime;
            yield return null;
        }

        slowMotionDuration = 0; //Reset to 0 to prevent shorter duration for next time

        yield return StartCoroutine(ScaleTime(1)); //Reset time
    }

    IEnumerator ScaleTime(float target)
    {
        while (Time.timeScale != target)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, timeTransitionSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }
}