
using UnityEngine;

public class GameplayController : MonoBehaviour, IDeckCardClickHandler, IGridCardClickHandler
{
    [@SerializeField] private GameObject cardPrefab;
    public CardData[] allCardData;    //Use auto fill button

    [@SerializeField] private CardGrid cardGrid;

    [@SerializeField] private WastePile wastePile;

    [@SerializeField] private Deck deck;

    [@SerializeField] private GameUI gameUI;

    private CardDataHandler cardDataHandler;
    private ICardFactory cardFactory;
    private CardValidator cardValidator;
    private GamePlayScoringSystem scoringSystem;
    private HintManager hintManager;

    void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
        InitGamePlay();
        InitGameUI();

        await wastePile.ReceiveCardFromDeck(deck.PopTopCard());
        SetReadyForPlayerInput(true);
    }

    private void InitGamePlay()
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


        cardValidator = new CardValidator();
        scoringSystem = new GamePlayScoringSystem();
        hintManager = new HintManager(cardValidator, cardGrid, deck, wastePile);
    }

    private void InitGameUI()
    {
        gameUI.Initialize(scoringSystem);
        gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
    }


    public async void HandleDeckCardClick(CardScript card)
    {
        if (IsUserGamePlayInteractionsBlocked) return;
        if (deck.HasCards())
        {
            SetReadyForPlayerInput(false);
            await wastePile.ReceiveCardFromDeck(deck.PopTopCard());
            scoringSystem.ResetSequence();
            gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
            SetReadyForPlayerInput(true);
        }
        else
        {
            Debug.Log("Card cannot be collected");
        }
    }

    public async void HandleGridCardClick(CardScript card)
    {
        if (IsUserGamePlayInteractionsBlocked) return;
        if (cardValidator.IsValidCardCollection(wastePile.GetTopCard(), card))
        {
            card.onCardClicked.RemoveAllListeners();
            SetReadyForPlayerInput(false);
            card.IsCollected = true;
            await wastePile.AddCardToWastePile(card);
            scoringSystem.AddCardToSequence(card.GetRank());
            await cardGrid.CheckForPossibleCardFlips();
            SetReadyForPlayerInput(true);
            CheckForGameOver();
        }
        else
        {
            card.ShakeCard();
            Debug.Log("Card cannot be collected");
        }
    }

    public void OnExtraDeckCardGranted()
    {
        deck.AddExtraCardsFromData(cardDataHandler.GetNRandomCards(NO_OF_EXTRA_DECK_CARDS_GRANTED), cardPrefab);
        gameUI.HandleBuyDeckBtnVisibility(deck.GetDeckCardCount());
    }


    public void OnHintGranted()
    {
        hintManager.RevealHint();
    }


    private void SetReadyForPlayerInput(bool IsReady)
    {
        IsUserGamePlayInteractionsBlocked = !IsReady;

        if (IsReady)
        {
            gameUI.OnGameplayInteractionResumed();
        }
        else
        {
            gameUI.OnGameplayInteractionBlocked();
        }
    }

    private void CheckForGameOver()
    {
        if (cardGrid.GetFlippedCards().Count == 0)
        {
            SetReadyForPlayerInput(false);
            gameUI.OnGameOver();
        }
    }

    private bool IsUserGamePlayInteractionsBlocked = true;
    private const int NO_OF_EXTRA_DECK_CARDS_GRANTED = 5;
}
