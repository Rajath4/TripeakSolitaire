using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    public Button buyDeckButton;

    public GameTimer gameTimer;

    public GameObject leaderboardPopupPrefab;

        public GameObject gameOverPopupPrefab; 


    public Transform popupParent; 

    public Button hintButton;

    private GamePlayScoringSystem scoringSystem;  // Reference to the ScoringSystem


    public void Initialize(GamePlayScoringSystem scoringSystem)
    {
        this.scoringSystem = scoringSystem;

        // Subscribe to score changes
        scoringSystem.OnScoreChanged += UpdateScoreDisplay;

        InitScoreDisplay();

        gameTimer.StartTimer();
    }

    private void OnDestroy()
    {
        if(scoringSystem == null) return;
        // Unsubscribe to prevent memory leaks
        scoringSystem.OnScoreChanged -= UpdateScoreDisplay;
    }

    // Update the score display whenever the score changes
    private void UpdateScoreDisplay(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
        UpdateHighScore(newScore);
    }

    // Update the high score display
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

    public void HandleBuyDeckBtnVisibility(int deckCardCount)
    {
        buyDeckButton.gameObject.SetActive(deckCardCount <= 0);
    }

    public void OnLeaderboardButtonClicked()
    {
        LeaderboardFullScreenPopup leaderboardPopup = LeaderboardFullScreenPopup.CreateInstance(leaderboardPopupPrefab,popupParent);
        leaderboardPopup.Show();
    }

    public void OnGameOver()
    {
        gameTimer.StopTimer();

        GameOverFullScreenPopup gameOverPopup = GameOverFullScreenPopup.CreateInstance(gameOverPopupPrefab,popupParent);
        gameOverPopup.Show();
    }

    public void OnGameplayInteractionBlocked()
    {
        hintButton.gameObject.SetActive(false);
    }
    
    public void OnGameplayInteractionResumed()
    {
        hintButton.gameObject.SetActive(true);
    }
}
