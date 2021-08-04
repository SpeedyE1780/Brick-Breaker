public class ScoringManager : Singleton<ScoringManager>
{
    private int score;
    
    void Start()
    {
        score = 0;
        UIManager.Instance.SetScore(score);
    }

    private void OnEnable() => EventManager.EAddScore += AddScore;

    private void OnDisable() => EventManager.EAddScore -= AddScore;

    void AddScore(int points)
    {
        score += points;
        UIManager.Instance.SetScore(score);
    }

    public void ResetScore()
    {
        score = 0;
        UIManager.Instance.SetScore(score);
    }
}