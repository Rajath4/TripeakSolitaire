using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCTAHolder : MonoBehaviour
{
    [@SerializeField] private Button buyDeckButton;
    [@SerializeField] private Button hintButton;
 
    public void HandleBuyDeckBtnVisibility(int deckCardCount)
    {
        buyDeckButton.gameObject.SetActive(deckCardCount <= 0);
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
