using UnityEngine;

public class CardFactory : ICardFactory
{
    public CardScript CreateCard(CardData cardData, GameObject cardPrefab, Transform parent)
    {
        GameObject cardObj = GameObject.Instantiate(cardPrefab, parent);
        CardScript cardScript = cardObj.GetComponent<CardScript>();
        cardScript.InitializeCard(cardData);
        return cardScript;
    }

    public CardScript CreateDeckCard(CardData cardData, GameObject cardPrefab, Transform parent)
    {
        CardScript cardScript = CreateCard(cardData, cardPrefab, parent);
        cardScript.IsDeckCard = true;
        return cardScript;
    }
}



