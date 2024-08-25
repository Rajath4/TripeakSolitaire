using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardDataHandler : ICardDataHandler
{
    private List<CardData> originalCards = new List<CardData>();
    private List<CardData> cardsInPlay = new List<CardData>();
    public CardDataHandler(CardData[] deckCards)
    {
        originalCards.AddRange(deckCards); // Initialize the deck with a predefined array of CardData
        cardsInPlay = originalCards.Select(card => CloneCard(card)).ToList(); // Clone each card if necessary
    }

    public void Shuffle()
    {
        int n = cardsInPlay.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            CardData value = cardsInPlay[k];
            cardsInPlay[k] = cardsInPlay[n];
            cardsInPlay[n] = value;
        }
    }

    public CardData DrawCard()
    {
        if (cardsInPlay.Count > 0)
        {
            CardData card = cardsInPlay[0];
            cardsInPlay.RemoveAt(0);
            return card;
        }
        return null;
    }

    public CardData[] GetAllCards()
    {
        return cardsInPlay.ToArray();
    }

    public CardData[] GetNRandomCards(int n)
    {
       //Create n random clone from original and send. make sure its not has duplicate cards
        List<CardData> randomCards = new List<CardData>();
        for (int i = 0; i < n; i++)
        {
            CardData randomCard = CloneCard(originalCards[Random.Range(0, originalCards.Count)]);
            if (!randomCards.Contains(randomCard))
            {
                randomCards.Add(randomCard);
            }
            else
            {
                i--;
            }
        }
        return randomCards.ToArray();
    }

    private CardData CloneCard(CardData original)
    {
        CardData clone = ScriptableObject.CreateInstance<CardData>();
        clone.name = original.name;
        clone.Rank = original.Rank;
        clone.Suit = original.Suit;
        clone.IsFaceUp = original.IsFaceUp;
        clone.FaceUpSprite = original.FaceUpSprite;
        clone.FaceDownSprite = original.FaceDownSprite;
        return clone;
    }

}
