using System.Collections.Generic;
using UnityEngine;

public class HintManager
{
    private CardValidator cardValidator;
    private CardGrid cardGrid;
    private Deck deck;
    private WastePile wastePile;

    public HintManager(CardValidator validator, CardGrid grid, Deck deck, WastePile wastePile)
    {
        this.cardValidator = validator;
        this.cardGrid = grid;
        this.deck = deck;
        this.wastePile = wastePile;
    }

    // Entry point for revealing hints
    public void RevealHint()
    {
        Debug.Log("RevealHint called");

        var eligibleCards = GetEligibleCards();
        if (eligibleCards.Count > 0)
        {
            Debug.Log("Shaking eligible cards");
            ShakeCards(eligibleCards);
        }else if (deck.HasCards())
        {
            Debug.Log("Shaking top card of the deck");
            deck.ShakeTopCard();
        }
        else
        {
            Debug.Log("No moves available and no cards in deck!");
        }
    }

    // Retrieves all eligible cards for a hint based on validation rules
    private List<CardScript> GetEligibleCards()
    {
        var eligibleCards = new List<CardScript>();
        foreach (var card in cardGrid.GetFlippedCards())
        {
            if (cardValidator.IsValidCardCollection(wastePile.GetTopCard(), card))
            {
                eligibleCards.Add(card);
            }
        }
        return eligibleCards;
    }

    // Triggers a shake animation on all eligible cards
    private void ShakeCards(List<CardScript> cards)
    {
        foreach (var card in cards)
        {
            if (card != null) // Check if the card object is not null
            {
                card.ShakeCard();
            }
        }
    }
}
