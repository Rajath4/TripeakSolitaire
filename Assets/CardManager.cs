using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static CardGrid;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector

    private List<List<CardScript>> cardsToPick = new List<List<CardScript>>();

    public Transform deckPosition;
    public Transform wastePilePosition;

    public CardGrid gridCardContainer;
    private Deck deck;

    private CardDataHandler cardDataHandler;

    private CardScript wastePileTopCard;


    void Start()
    {
        cardDataHandler = new CardDataHandler(allCardData);
        cardDataHandler.Shuffle();

        deck = new Deck();

        SetupCards();
        ComputeDependencies();

        CheckForPossibleCardFlips();

        SetupDeckCards();
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

    private void SetupDeckCards()
    {
        int currentIndex = 0;
        foreach (CardData cardData in cardDataHandler.GetAllCards())
        {
            CardScript cardScript = GetCardScript(cardData, deckPosition);
            RegisterDeckCardClickHandler(cardScript); // Register the card for click events
            deck.AddCardScript(cardScript);
            cardScript.IsDeckCard = true;
            currentIndex++;
        }
    }

    private async void MoveToDeckCardToWastePile()
    {
        CardScript currentWastePileTopCard = wastePileTopCard;

        wastePileTopCard = deck.GetCardScriptAtTop();
        wastePileTopCard.FlipWithAnimation();
        // Vector3 wastePilePositionZOffset = new Vector3(wastePilePosition.position.x, wastePilePosition.position.y, wastePilePosition.position.z - wastePile.Count * 0.1f);
        // Transform wastePileTransform = wastePilePosition;
        // wastePileTransform.position = wastePilePositionZOffset;
        await wastePileTopCard.MoveToDestination(wastePilePosition);
        wastePileTopCard.transform.SetParent(wastePilePosition);
        wastePileTopCard.onCardClicked.RemoveListener(HandleDeckCardClick);

        if (currentWastePileTopCard != null)
        {
            Destroy(currentWastePileTopCard.gameObject);
        }
    }

    private CardScript GetCardScript(CardData cardData, Transform parent)
    {

        GameObject cardObj = Instantiate(cardPrefab, parent);
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
            card.Flip();
            card.IsCollected = true;
            Debug.Log($"Card clicked: {card.cardData.name}");

            // card.MoveToDestination(wastePilePosition);
            CheckForPossibleCardFlips();
            await PlayCardToWastePileAsync(card);
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

        CardScript topCard = wastePileTopCard;
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
    public async Task PlayCardToWastePileAsync(CardScript card)
    {
        CardScript cardRemovedFromWastePile = wastePileTopCard;
        wastePileTopCard = card;

        // Vector3 wastePilePositionZOffset = new Vector3(wastePilePosition.position.x, wastePilePosition.position.y, wastePilePosition.position.z - wastePile.Count * 0.1f);
        // Transform wastePileTransform = wastePilePosition;
        // wastePileTransform.position = wastePilePosition;
        await card.MoveToDestination(wastePilePosition);
        card.transform.SetParent(wastePilePosition);

        Destroy(cardRemovedFromWastePile.gameObject);
    }
}
