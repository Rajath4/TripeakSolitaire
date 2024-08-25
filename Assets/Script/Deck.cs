using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Transform parent;

    private IDeckCardClickHandler clickHandler;
    private ICardFactory cardFactory;

    private readonly List<CardScript> deckCards = new List<CardScript>();

    public void Initialize(ICardFactory cardFactory, IDeckCardClickHandler clickHandler)
    {
        this.cardFactory = cardFactory;
        this.clickHandler = clickHandler;
    }

    public void SetupDeckCards(CardData[] cardDatas, GameObject cardPrefab)
    {
        foreach (CardData cardData in cardDatas)
        {
            CardScript cardScript = cardFactory.CreateDeckCard(cardData, cardPrefab, parent);
            cardScript.onCardClicked.AddListener(clickHandler.HandleDeckCardClick);
            AddCard(cardScript);
        }
    }

    public void AddExtraCardsFromData(CardData[] cardDatas, GameObject cardPrefab)
    {
        foreach (CardData cardData in cardDatas)
        {
            if (cardData != null)
            {
                CardScript cardScript = cardFactory.CreateDeckCard(cardData, cardPrefab, parent);
                cardScript.onCardClicked.AddListener(clickHandler.HandleDeckCardClick);
                AddCard(cardScript);
            }
        }
    }

    public bool HasCards()
    {
        return deckCards.Count > 0;
    }

    private void AddCard(CardScript cardScript)
    {
        deckCards.Add(cardScript);
    }

    public CardScript GetCardAtTop()
    {
        if (deckCards.Count > 0)
        {
            CardScript cardScript = deckCards[deckCards.Count - 1];
            deckCards.RemoveAt(deckCards.Count - 1);
            return cardScript;
        }
        else
        {
            return null;
        }
    }

    public int GetDeckCardCount()
    {
        return deckCards.Count;
    }
}
