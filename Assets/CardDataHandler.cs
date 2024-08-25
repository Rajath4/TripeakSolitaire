using System.Collections.Generic;
using UnityEngine;

public class CardDataHandler
{
    private List<CardData> cards = new List<CardData>();

    public CardDataHandler(CardData[] deckCards)
    {
        cards.AddRange(deckCards); // Initialize the deck with a predefined array of CardData
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

    public CardData[] GetAllCards()
    {
        return cards.ToArray();
    }
}
