public class CardValidator
{
    public bool IsValidCardCollection(CardScript wastePileCard, CardScript cardToCollect)
    {

        Rank collectedCardRank = cardToCollect.GetRank();
        Rank wastePileCardRank = wastePileCard.GetRank();

        if (collectedCardRank == Rank.Ace)
        {
            // Ace can be placed on Two or King
            return wastePileCardRank == Rank.Two || wastePileCardRank == Rank.King;
        }
        else if (collectedCardRank == Rank.Two)
        {
            // Two can be placed on Ace or Three
            return wastePileCardRank == Rank.Ace || wastePileCardRank == Rank.Three;
        }
        // else if (collectedCardRank == Rank.King) //TODO: Check it
        // {
        //     // King can be placed on Queen or Ace
        //     return wastePileCardRank == Rank.Queen || wastePileCardRank == Rank.Ace;
        // }
        else
        {
            // Normal behavior: check next and previous rank in the enum
            return collectedCardRank == wastePileCardRank + 1 || collectedCardRank == wastePileCardRank - 1;
        }

    }
}