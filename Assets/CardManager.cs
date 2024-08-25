using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static GridCardContainer;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector

    private List<List<CardScript>> cardsToPick = new List<List<CardScript>>();

    public Transform deckPosition;
    public Transform wastePilePosition;

    public GridCardContainer gridCardContainer;
    private Deck deck;
    public List<CardScript> wastePile = new List<CardScript>();


    void Start()
    {
        deck = new Deck(allCardData); // Initialize the deck with all available CardData
        // DealCards();
        SetupCards();
        ComputeDependencies();
        CheckForPossibleCardFlips();



        SetupDeckCards();
        MoveToDeckCardToWastePile();
    }


    void SetupCards()
    {
        int currentIndex = 0;
        float zOffset = -0.1f;
        int currentRow = 0;

        foreach (RowData row in gridCardContainer.rows)
        {
            List<CardScript> currentRowCardScripts = new List<CardScript>();
            foreach (Transform pos in row.positions)
            {
                Vector3 positionWithZOffset = new Vector3(pos.position.x, pos.position.y, pos.position.z + currentRow * zOffset);

                GameObject cardObj = Instantiate(cardPrefab, deckPosition);
                CardScript cardScript = cardObj.GetComponent<CardScript>();
                cardScript.InitializeCard(allCardData[currentIndex]);
                RegisterCardClickHandler(cardScript); // Register the card for click events
                currentRowCardScripts.Add(cardScript);
                currentIndex++;
                Debug.Log($"Card {cardScript.cardData.name} created at {positionWithZOffset}");
            }
            cardsToPick.Add(currentRowCardScripts);
            currentRow++;
        }
    }

    private void SetupDeckCards()
    {
        int currentIndex = 0;
        foreach (CardData cardData in allCardData)
        {
            CardScript cardScript = GetCardScript(cardData);
            cardScript.transform.position = deckPosition.position;
            RegisterDeckCardClickHandler(cardScript); // Register the card for click events
            deck.AddCardScript(cardScript);
            currentIndex++;
        }
    }

    private void MoveToDeckCardToWastePile()
    {
        CardScript cardScript = deck.GetCardScriptAtTop();
        wastePile.Add(cardScript);
        cardScript.FlipWithAnimation();
        Vector3 wastePilePositionZOffset = new Vector3(wastePilePosition.position.x, wastePilePosition.position.y, wastePilePosition.position.z - wastePile.Count * 0.1f);
        Transform wastePileTransform = wastePilePosition;
        wastePileTransform.position = wastePilePositionZOffset;
        cardScript.MoveToDestination(wastePileTransform);
    }

    private CardScript GetCardScript(CardData cardData)
    {

        GameObject cardObj = Instantiate(cardPrefab);
        CardScript cardScript = cardObj.GetComponent<CardScript>();
        cardScript.InitializeCard(cardData);
        return cardScript;
    }

    void ComputeDependencies()
    {
        for (int rowIndex = 0; rowIndex < cardsToPick.Count - 1; rowIndex++)
        {
            List<CardScript> row = cardsToPick[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                CardScript card = row[columnIndex];
                int[] dependentIndices = gridCardContainer.GetDependentsIndex(rowIndex, columnIndex);
                int nextRowIndex = rowIndex + 1;
                foreach (int dependentIndicesColumn in dependentIndices)
                {
                    card.dependsOnCards.Add(cardsToPick[nextRowIndex][dependentIndicesColumn]);
                }
            }
        }
    }

    public void RegisterDeckCardClickHandler(CardScript card)
    {
        card.onCardClicked.AddListener(HandleDeckCardClick);
    }

    private void HandleDeckCardClick(CardScript card)
    {
        if (deck.HasCards())
        {
            MoveToDeckCardToWastePile();
        }
        else
        {
            Debug.Log("Card cannot be collected");
        }
    }


    public void RegisterCardClickHandler(CardScript card)
    {
        card.onCardClicked.AddListener(HandleCardClick);
    }


    private void HandleCardClick(CardScript card)
    {
        if (CanCardBeCollected(card))
        {
            card.Flip();
            card.IsCollected = true;
            Debug.Log($"Card clicked: {card.cardData.name}");

            card.MoveToDestination(wastePilePosition);
            CheckForPossibleCardFlips();
            PlayCardToWastePile(card);
        }
        else
        {
            Debug.Log("Card cannot be collected");
        }


    }


    public List<CardScript> GetFlippedCards()
    {
        List<CardScript> flippedCards = new List<CardScript>();
        foreach (var row in cardsToPick)
        {
            foreach (var card in row)
            {
                if (card.cardData.IsFaceUp)
                {
                    flippedCards.Add(card);
                }
            }
        }
        return flippedCards;
    }

    private void CheckForPossibleCardFlips()
    {
        foreach (var row in cardsToPick)
        {
            foreach (var card in row)
            {
                card.handleFlipIfEligible();
            }
        }
    }

    public bool CanCardBeCollected(CardScript card)
    {
        if (wastePile.Count == 0)
        {
            Debug.LogError("Waste pile is empty");
        }
        CardScript topCard = wastePile[wastePile.Count - 1];
        Rank cardRank = card.cardData.Rank;
        Rank topCardRank = topCard.cardData.Rank;

        if (cardRank == Rank.Ace)
        {
            // Ace can be placed on Two or King
            return topCardRank == Rank.Two || topCardRank == Rank.King;
        }
        else if (cardRank == Rank.Two)
        {
            // Two can be placed on Ace or Three
            return topCardRank == Rank.Ace || topCardRank == Rank.Three;
        }
        // else if (cardRank == Rank.King) //TODO: Check it
        // {
        //     // King can be placed on Queen or Ace
        //     return topCardRank == Rank.Queen || topCardRank == Rank.Ace;
        // }
        else
        {
            // Normal behavior: check next and previous rank in the enum
            return cardRank == topCardRank + 1 || cardRank == topCardRank - 1;
        }

    }
    public void PlayCardToWastePile(CardScript card)
    {
        CardScript topCard = wastePile[wastePile.Count - 1];
        wastePile.RemoveAt(wastePile.Count - 1); // Remove the top card from the waste pile
        Destroy(topCard.gameObject);
        wastePile.Add(card); // Add the card to the waste pile
    }
}
