using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [@SerializeField] private Button buyDeckButton;
    [@SerializeField] private GameTimer gameTimer;
    [@SerializeField] private PopupHandler popupHandler;
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
        popupHandler.ShowLeaderboardPopup();
    }

    public void OnGameOver()
    {
        gameTimer.StopTimer();
        popupHandler.ShowGameOverPopup();
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
