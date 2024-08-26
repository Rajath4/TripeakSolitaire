using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{

    [@SerializeField] private GameObject leaderboardPopupPrefab;

    [@SerializeField] private GameObject gameOverPopupPrefab;

    [@SerializeField] private Transform popupParent;


    public void ShowLeaderboardPopup()
    {
        LeaderboardFullScreenPopup leaderboardPopup = LeaderboardFullScreenPopup.CreateInstance(leaderboardPopupPrefab, popupParent);
        leaderboardPopup.Show();
    }

    public void ShowGameOverPopup()
    {
        GameOverFullScreenPopup gameOverPopup = GameOverFullScreenPopup.CreateInstance(gameOverPopupPrefab, popupParent);
        gameOverPopup.Show();
    }
}
