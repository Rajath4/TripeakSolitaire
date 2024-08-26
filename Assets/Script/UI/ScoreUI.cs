using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [@SerializeField]
    private TMP_Text scoreText;

    [@SerializeField]
    private TMP_Text highScoreText;

    private GamePlayScoringSystem scoringSystem;  // Reference to the ScoringSystem


    public void Initialize(GamePlayScoringSystem scoringSystem)
    {
        this.scoringSystem = scoringSystem;

        // Subscribe to score changes
        scoringSystem.OnScoreChanged += UpdateScoreDisplay;

        InitScoreDisplay();
    }

    private void OnDestroy()
    {
        if (scoringSystem == null) return;
        // Unsubscribe to prevent memory leaks
        scoringSystem.OnScoreChanged -= UpdateScoreDisplay;
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
        UpdateHighScore(newScore);
    }

    private void UpdateHighScore(int newScore)
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (newScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", newScore);
            PlayerPrefs.Save();
            highScoreText.text = $"High Score: {newScore}";
        }
    }

    private void InitScoreDisplay()
    {
        scoreText.text = "Score: 0";
        highScoreText.text = $"High Score: {PlayerPrefs.GetInt("HighScore", 0)}";
    }
}
