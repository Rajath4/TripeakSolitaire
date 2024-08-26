using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [@SerializeField] private Button buyDeckButton;

    [@SerializeField] private GameTimer gameTimer;

    [@SerializeField] private GameObject leaderboardPopupPrefab;

    [@SerializeField] private GameObject gameOverPopupPrefab;

    [@SerializeField] private Transform popupParent;

    [@SerializeField] private Button hintButton;

    [@SerializeField] private ScoreUI scoreUI;

    public void Initialize(GamePlayScoringSystem scoringSystem)
    {

        scoreUI.Initialize(scoringSystem);
        gameTimer.StartTimer();
    }

    public void HandleBuyDeckBtnVisibility(int deckCardCount)
    {
        buyDeckButton.gameObject.SetActive(deckCardCount <= 0);
    }

    public void OnLeaderboardButtonClicked()
    {
        LeaderboardFullScreenPopup leaderboardPopup = LeaderboardFullScreenPopup.CreateInstance(leaderboardPopupPrefab, popupParent);
        leaderboardPopup.Show();
    }

    public void OnGameOver()
    {
        gameTimer.StopTimer();

        GameOverFullScreenPopup gameOverPopup = GameOverFullScreenPopup.CreateInstance(gameOverPopupPrefab, popupParent);
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
