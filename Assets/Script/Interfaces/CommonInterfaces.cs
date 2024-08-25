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
