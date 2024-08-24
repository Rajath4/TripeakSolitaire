using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform[] cardPositions; // Positions to spawn cards
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector

    private Deck deck;

    void Start()
    {
        deck = new Deck(allCardData); // Initialize the deck with all available CardData
        DealCards();
    }

    private void DealCards()
    {
        foreach (Transform position in cardPositions)
        {
            CardData cardData = deck.DrawCard();
            if (cardData != null)
            {
                GameObject cardObject = Instantiate(cardPrefab, position.position, Quaternion.identity, position);
                CardScript cardScript = cardObject.GetComponent<CardScript>();
                cardScript.InitializeCard(cardData); // Initialize the card with CardData
            }
        }
    }

    // Add any additional methods needed for gameplay, like reshuffling, resetting the deck, etc.
}
