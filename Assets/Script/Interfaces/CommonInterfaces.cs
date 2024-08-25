using UnityEngine;

public interface ICardFactory
{
    CardScript CreateCard(CardData cardData, GameObject cardPrefab, Transform parent);
    CardScript CreateDeckCard(CardData cardData, GameObject cardPrefab, Transform parent);
}

public interface IDeckCardClickHandler
{
    void HandleDeckCardClick(CardScript card);
}

public interface IGridCardClickHandler
{
    void HandleGridCardClick(CardScript card);
}


public interface ICardDataHandler
{
    CardData DrawCard();
    void Shuffle();
    CardData[] GetAllCards();
}

