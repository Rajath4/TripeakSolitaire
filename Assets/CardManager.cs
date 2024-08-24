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
                RegisterCardClickHandler(cardScript); // Register the card for click events
            }
        }
    }

    public void RegisterCardClickHandler(CardScript card)
    {
        card.onCardClicked.AddListener(HandleCardClick);
    }

    private void HandleCardClick(CardScript card)
    {
        Debug.Log($"Card clicked: {card.cardData.name}");
    }

}
