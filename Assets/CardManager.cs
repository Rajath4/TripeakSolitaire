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

    void Start()
    {
        deck = new Deck(allCardData); // Initialize the deck with all available CardData
        // DealCards();
        SetupCards();
        ComputeDependencies();
        CheckForPossibleCardFlips();
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

                GameObject cardObj = Instantiate(cardPrefab, positionWithZOffset, Quaternion.identity);
                CardScript cardScript = cardObj.GetComponent<CardScript>();
                cardScript.InitializeCard(allCardData[currentIndex]);
                RegisterCardClickHandler(cardScript); // Register the card for click events
                currentRowCardScripts.Add(cardScript);
                currentIndex++;
            }
            cardsToPick.Add(currentRowCardScripts);
            currentRow++;
        }
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


    public void RegisterCardClickHandler(CardScript card)
    {
        card.onCardClicked.AddListener(HandleCardClick);
    }

    private void HandleCardClick(CardScript card)
    {
        card.Flip();
        card.IsCollected = true;
        Debug.Log($"Card clicked: {card.cardData.name}");

        card.MoveToDestination(wastePilePosition);
        CheckForPossibleCardFlips();
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

}
