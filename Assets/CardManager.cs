using System.Threading.Tasks;
using UnityEngine;

public class CardManager : MonoBehaviour, IDeckCardClickHandler,IGridCardClickHandler
{
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector


    public CardGrid cardGrid;

    private CardDataHandler cardDataHandler;

    public WastePile wastePile;
    public Deck deck;

    private ICardFactory cardFactory;

    void Start()
    {
        cardDataHandler = new CardDataHandler(allCardData);
        cardDataHandler.Shuffle();

        cardFactory = new CardFactory();

        cardGrid.Initialize(cardDataHandler, cardFactory, this);
        deck.Initialize(cardFactory, this);

        cardGrid.SetupCards(cardPrefab);
        cardGrid.ComputeDependencies();

        cardGrid.CheckForPossibleCardFlips();
        deck.SetupDeckCards(cardDataHandler.GetAllCards(), cardPrefab);

        MoveToDeckCardToWastePile();
    }

    private async Task MoveToDeckCardToWastePile()
    {
        CardScript card = deck.GetCardAtTop();
        card.FlipWithAnimation();
        await wastePile.AddCardToWastePile(card);
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

    public async void HandleGridCardClick(CardScript card)
    {
        if (CanCardBeCollected(card))
        {
            card.IsCollected = true;
            Debug.Log($"Card clicked: {card.cardData.name}");
            await wastePile.AddCardToWastePile(card);
            cardGrid.CheckForPossibleCardFlips();
        }
        else
        {
            Debug.Log("Card cannot be collected");
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
