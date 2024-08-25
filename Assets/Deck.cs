using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<CardData> cards = new List<CardData>();

    private List<CardScript> deckCards = new List<CardScript>();

    public Deck(CardData[] deckCards)
    {
        cards.AddRange(deckCards); // Initialize the deck with a predefined array of CardData
        Shuffle();
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            CardData value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public CardData DrawCard()
    {
        if (cards.Count > 0)
        {
            CardData card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        return null;
    }

 
    public bool HasCards()
    {
        return deckCards.Count > 0;
    }

    public void AddCardScript(CardScript cardScript)
    {
        deckCards.Add(cardScript);
    }



    public CardScript GetCardScriptAtTop()
    {
        if (deckCards.Count > 0)
        {
            CardScript cardScript = deckCards[deckCards.Count - 1];
            deckCards.RemoveAt(deckCards.Count - 1);
            return cardScript;
        }else{
            return null;
        }
    }
}
