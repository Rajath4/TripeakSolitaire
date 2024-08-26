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

    public GameUI gameUI;

    private GamePlayScoringSystem scoringSystem;
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
        scoringSystem = new GamePlayScoringSystem();

        InitGameUI();
    }

    private void InitGameUI()
    {
        gameUI.Initialize(scoringSystem);
        gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
    }


    public void HandleDeckCardClick(CardScript card)
    {
        if (deck.HasCards())
        {
            Debug.Log("Card MoveToDeckCardToWastePile");
            wastePile.ReceiveCardFromDeck(deck.GetCardAtTop());
            scoringSystem.ResetSequence();
            gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
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
            scoringSystem.AddCardToSequence(card.cardData.Rank);
            cardGrid.CheckForPossibleCardFlips();
        }
        else
        {
            Debug.Log("Card cannot be collected");
        }
    }

    public void OnExtraDeckCardGranted()
    {
        deck.AddExtraCardsFromData(cardDataHandler.GetNRandomCards(NO_OF_EXTRA_DECK_CARDS_GRANTED), cardPrefab);
        gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
    }

    private const int NO_OF_EXTRA_DECK_CARDS_GRANTED = 5;
}
