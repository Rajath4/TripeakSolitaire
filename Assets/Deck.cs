using System.Collections.Generic;
using UnityEngine;

public class Deck
{

    private List<CardScript> deckCards = new List<CardScript>();

    public Deck()
    {

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
        }
        else
        {
            return null;
        }
    }
}
