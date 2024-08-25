using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static CardGrid;

public class CardManager : MonoBehaviour, IDeckCardClickHandler
{
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector

    private List<List<CardScript>> cardsToPick = new List<List<CardScript>>();

    public CardGrid gridCardContainer;

    private CardDataHandler cardDataHandler;

    public WastePile wastePile;
    public Deck deck;

    private ICardFactory cardFactory;

    void Start()
    {
        cardDataHandler = new CardDataHandler(allCardData);
        cardDataHandler.Shuffle();

        cardFactory = new CardFactory();

        deck.Initialize(cardFactory, this);

        SetupCards();
        ComputeDependencies();

        CheckForPossibleCardFlips();
        deck.SetupDeckCards(cardDataHandler.GetAllCards(), cardPrefab);

        MoveToDeckCardToWastePile();
    }


    void SetupCards()
    {
        int currentIndex = 0;
        int currentRow = 0;

        foreach (RowData row in gridCardContainer.rows)
        {
            List<CardScript> currentRowCardScripts = new List<CardScript>();
            foreach (Transform pos in row.positions)
            {
                GameObject cardObj = Instantiate(cardPrefab, pos);
                CardScript cardScript = cardObj.GetComponent<CardScript>();
                cardScript.InitializeCard(cardDataHandler.DrawCard());
                RegisterCardClickHandler(cardScript); // Register the card for click events
                currentRowCardScripts.Add(cardScript);
                currentIndex++;
            }
            cardsToPick.Add(currentRowCardScripts);
            currentRow++;
        }
    }


    private async Task MoveToDeckCardToWastePile()
    {
        CardScript card = deck.GetCardAtTop();
        card.FlipWithAnimation();
        await wastePile.AddCardToWastePile(card);
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
                    card.AddDependency(cardsToPick[nextRowIndex][dependentIndicesColumn]);
                }
            }
        }
    }


    public void HandleDeckCardClick(CardScript card)
    {
        if (deck.HasCards())
        {
            Debug.Log("Card MoveToDeckCardToWastePile");
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


    private async void HandleCardClick(CardScript card)
    {
        if (CanCardBeCollected(card))
        {
            card.IsCollected = true;
            Debug.Log($"Card clicked: {card.cardData.name}");
            await wastePile.AddCardToWastePile(card);
            CheckForPossibleCardFlips();
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

        CardScript topCard = wastePile.GetTopCard();
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

}
