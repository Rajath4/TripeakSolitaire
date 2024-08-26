using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [@SerializeField] private GameTimer gameTimer;
    [@SerializeField] private PopupHandler popupHandler;
    [@SerializeField] private ScoreUI scoreUI;
    [@SerializeField] private GameplayCTAHolder gameplayCTAHolder;

    public void Initialize(GamePlayScoringSystem scoringSystem)
    {
        scoreUI.Initialize(scoringSystem);
        gameTimer.StartTimer();
    }

    public void HandleBuyDeckBtnVisibility(int deckCardCount)
    {
        gameplayCTAHolder.HandleBuyDeckBtnVisibility(deckCardCount);
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
        gameplayCTAHolder.OnGameplayInteractionBlocked();
    }

    public void OnGameplayInteractionResumed()
    {
        gameplayCTAHolder.OnGameplayInteractionResumed();
    }
}
