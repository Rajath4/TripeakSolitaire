using System.Threading.Tasks;
using UnityEngine;

public class GameplayController : MonoBehaviour, IDeckCardClickHandler, IGridCardClickHandler
{
    public GameObject cardPrefab;     // Card prefab which includes the CardScript
    public CardData[] allCardData;    // Array of all CardData ScriptableObjects, assigned via the inspector


    public CardGrid cardGrid;

    private CardDataHandler cardDataHandler;

    public WastePile wastePile;
    public Deck deck;

    private ICardFactory cardFactory;

    private CardValidator cardValidator;

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

        wastePile.ReceiveCardFromDeck(deck.GetCardAtTop());

        cardValidator = new CardValidator();
    }


    public void HandleDeckCardClick(CardScript card)
    {
        if (deck.HasCards())
        {
            Debug.Log("Card MoveToDeckCardToWastePile");
            wastePile.ReceiveCardFromDeck(deck.GetCardAtTop());
        }
        else
        {
            Debug.Log("Card cannot be collected");
        }
    }

    public async void HandleGridCardClick(CardScript card)
    {
        if (cardValidator.IsValidCardCollection(wastePile.GetTopCard(), card))
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
}
